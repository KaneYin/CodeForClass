#include "mpi.h"
#include <math.h>
#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[])
{
    int n, myid, numprocs, i;
    double PI25DT = 3.141592653589793238462643;
    double mypi, pi, h, sum, x;

    MPI_Init(&argc, &argv);
    MPI_Comm_size(MPI_COMM_WORLD, &numprocs);
    MPI_Comm_rank(MPI_COMM_WORLD, &myid);

    if (myid == 0) {
        if (argc < 2) {
            printf("Usage: mpirun -np <P> ./par_pi <n>\n");
            n = 0;
        } else {
            n = atoi(argv[1]);
        }
    }

    MPI_Bcast(&n, 1, MPI_INT, 0, MPI_COMM_WORLD);

    if (n > 0)
    {
        h = 1.0 / (double)n;
        sum = 0.0;
        for (i = myid + 1; i <= n; i += numprocs)
        {
            x = h * ((double)i - 0.5);
            sum += 4.0 / (1.0 + x * x);
        }
        mypi = h * sum;

        /* Hypercube recursive reduction using MPI_Send / MPI_Recv */
        {
            int k, partner, active = 1;
            int num_rounds = 0, temp = numprocs;
            double recv_val;
            MPI_Status status;

            while (temp > 1) { num_rounds++; temp >>= 1; }

            for (k = 0; k < num_rounds && active; k++) {
                partner = myid ^ (1 << k);
                if (myid & (1 << k)) {
                    MPI_Send(&mypi, 1, MPI_DOUBLE, partner, k, MPI_COMM_WORLD);
                    active = 0;
                } else {
                    MPI_Recv(&recv_val, 1, MPI_DOUBLE, partner, k, MPI_COMM_WORLD, &status);
                    mypi += recv_val;
                }
            }
            pi = mypi;
        }

        if (myid == 0)
            printf("pi is approximately %.16f, Error is %.16f\n", pi, fabs(pi - PI25DT));
    }

    MPI_Finalize();
    return 0;
}
