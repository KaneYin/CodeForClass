#include <stdio.h>
#include <stdlib.h>
#include <sys/time.h>
#include <time.h>
#include <string.h>
#include <errno.h>

static inline double now_us(void) {
    struct timeval tv;
    gettimeofday(&tv, NULL);
    return (double)tv.tv_sec * 1e6 + (double)tv.tv_usec;
}

static void *xaligned_alloc(size_t alignment, size_t size) {
#if defined(_ISOC11_SOURCE)
    void *p = aligned_alloc(alignment, (size + alignment - 1) / alignment * alignment);
    return p;
#else
    void *p = NULL;
    if (posix_memalign(&p, alignment, size) != 0) return NULL;
    return p;
#endif
}

static double *alloc_matrix(int n) {
    size_t bytes = (size_t)n * (size_t)n * sizeof(double);
    double *m = (double *)xaligned_alloc(64, bytes);
    if (!m) {
        // Fallback
        m = (double *)malloc(bytes);
    }
    if (!m) {
        fprintf(stderr, "Allocation failed for %zu bytes (n=%d)\n", bytes, n);
        exit(1);
    }
    return m;
}

static void init_random(double *m, int n) {
    size_t nn = (size_t)n * (size_t)n;
    for (size_t i = 0; i < nn; i++) {
        m[i] = (double)rand() / (double)RAND_MAX;
    }
}

static void zero_matrix(double *m, int n) {
    memset(m, 0, (size_t)n * (size_t)n * sizeof(double));
}

// Matrix multiply baselines
// Original i-j-k version
static void mm_original(const double *A, const double *B, double *C, int n)
{
    for (int i = 0; i < n; i++)
    {
        int in = i * n;
        for (int j = 0; j < n; j++)
        {
            double sum = 0.0;
            for (int k = 0; k < n; k++)
            {
                sum += A[in + k] * B[k * n + j];
            }
            C[in + j] = sum;
        }
    }
}

static inline int imin(int a, int b) { return a < b ? a : b; }

// Cache blocking / tiling version
static void mm_blocked(const double *A, const double *B, double *C, int n, int Bsz) {
    zero_matrix(C, n);

    for (int i0 = 0; i0 < n; i0 += Bsz) {
        int i_max = imin(i0 + Bsz, n);
        for (int k0 = 0; k0 < n; k0 += Bsz) {
            int k_max = imin(k0 + Bsz, n);
            for (int j0 = 0; j0 < n; j0 += Bsz) {
                int j_max = imin(j0 + Bsz, n);

                for (int i = i0; i < i_max; i++) {
                    int in = i * n;
                    for (int k = k0; k < k_max; k++) {
                        double a = A[in + k];
                        int kn = k * n;
                        for (int j = j0; j < j_max; j++) {
                            C[in + j] += a * B[kn + j];
                        }
                    }
                }
            }
        }
    }
}

// Experiments
static double run_single_original(int n) {
    double *A = alloc_matrix(n);
    double *B = alloc_matrix(n);
    double *C = alloc_matrix(n);

    init_random(A, n);
    init_random(B, n);

    double t0 = now_us();
    mm_original(A, B, C, n);
    double t1 = now_us();

    free(A);
    free(B);
    free(C);

    return t1 - t0;
}

static double run_single_blocked(int n, int Bsz) {
    double *A = alloc_matrix(n);
    double *B = alloc_matrix(n);
    double *C = alloc_matrix(n);

    init_random(A, n);
    init_random(B, n);

    double t0 = now_us();
    mm_blocked(A, B, C, n, Bsz);
    double t1 = now_us();

    free(A);
    free(B);
    free(C);

    return t1 - t0;
}

int main(int argc, char **argv) {
    // Seed BEFORE any initialization
    srand((unsigned)time(NULL));

    // "single original N" or "single blocked N B"
    if (strcmp(argv[1], "single") == 0) {
        if (argc < 4) {
            return 1;
        }
        const char *mode = argv[2];
        int n = atoi(argv[3]);
        if (n <= 0) {
            fprintf(stderr, "Invalid N: %s\n", argv[3]);
            return 1;
        }

        if (strcmp(mode, "original") == 0) {
            double dt = run_single_original(n);
            printf("original,%d,0,%.0f\n", n, dt);
            return 0;
        } else if (strcmp(mode, "blocked") == 0) {
            if (argc < 5) {
                return 1;
            }
            int Bsz = atoi(argv[4]);
            if (Bsz <= 0) {
                fprintf(stderr, "Invalid B: %s\n", argv[4]);
                return 1;
            }
            double dt = run_single_blocked(n, Bsz);
            printf("blocked,%d,%d,%.0f\n", n, Bsz, dt);
            return 0;
        } else {
            fprintf(stderr, "Unknown mode: %s\n", mode);
            return 1;
        }
    }

    // exp2: original sweep N=16..8192, powers of 2
    if (strcmp(argv[1], "exp2") == 0) {
        for (int n = 16; n <= 8192; n <<= 1) {
            double dt = run_single_original(n);
            printf("original,%d,0,%.0f\n", n, dt);
            fflush(stdout);
        }
        return 0;
    }

    // exp3: blocked with B=16 sweep N=16..8192
    if (strcmp(argv[1], "exp3") == 0) {
        int Bsz = 16;
        for (int n = 16; n <= 8192; n <<= 1) {
            double dt = run_single_blocked(n, Bsz);
            printf("blocked,%d,%d,%.0f\n", n, Bsz, dt);
            fflush(stdout);
        }
        return 0;
    }

    // exp4: blocked with N=4096 fixed, sweep B=4..512, powers of 2
    if (strcmp(argv[1], "exp4") == 0) {
        int n = 4096;
        for (int Bsz = 4; Bsz <= 512; Bsz <<= 1) {
            double dt = run_single_blocked(n, Bsz);
            printf("blocked,%d,%d,%.0f\n", n, Bsz, dt);
            fflush(stdout);
        }
        return 0;
    }

    fprintf(stderr, "Unknown command: %s\n", argv[1]);
    return 1;
}