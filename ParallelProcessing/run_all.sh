#!/bin/bash
#
# Usage:
#   Step 1: ./run_all.sh compile
#   Step 2: ./run_all.sh baseline
#   Step 3: ./run_all.sh rest
#   Step 4: ./run_all.sh collect
#
# 检查任务状态: squeue -u $USER
#

set -e

MPICC=/usr/mpi/gcc/openmpi-4.1.5rc2/bin/mpicc
LAUNCHER=srun

OUTFILE1="par_pi_op_ydong.txt"
OUTFILE2="par_pi_op_simple_ydong.txt"

RESULT_DIR="job_results"
N_VALUES="1048576 8388608 67108864"

mkdir -p "${RESULT_DIR}"

compile() {
    sbatch <<EOF
#!/bin/bash
set -e
#SBATCH --job-name=compile
#SBATCH --partition=batch
#SBATCH --nodes=1
#SBATCH --ntasks=1
#SBATCH --time=00:05:00
#SBATCH --output=slurm_compile.out

${MPICC} -o par_pi par_pi.c -lm
${MPICC} -o par_pi_simple par_pi_simple.c -lm
echo "Compile done."
EOF
    echo "Compile job submitted. Wait for it to finish, then run: ./run_all.sh baseline"
}

submit_job() {
    local PROG=$1
    local N=$2
    local P=$3
    local P1=$4
    local P2=$5
    local COMBO=$6
    local OUTFILE=$7
    local JOBNAME="${PROG}_n${N}_P${P}_${COMBO}"
    local RESULT_FILE="${RESULT_DIR}/${JOBNAME}.txt"

    sbatch <<EOF
#!/bin/bash
set -e
#SBATCH --job-name=${JOBNAME}
#SBATCH --partition=batch
#SBATCH --nodes=${P1}
#SBATCH --ntasks-per-node=${P2}
#SBATCH --ntasks=${P}
#SBATCH --cpus-per-task=1
#SBATCH --time=00:10:00
#SBATCH --output=slurm_${JOBNAME}.out

echo "program=${PROG}, N=${N}, P=${P}, nodes=${P1}, ntasks_per_node=${P2}, combo=${COMBO}" > ${RESULT_FILE}
${LAUNCHER} --mpi=pmix ./${PROG} ${N} 2>&1 \
    | sed '/libnuma\.so/d;/LOG_CAT_SBGP/d' \
    >> ${RESULT_FILE}
EOF
    echo "Submitted: ${JOBNAME} (nodes=${P1}, ntasks-per-node=${P2})"
}

baseline() {
    rm -f "${OUTFILE1}" "${OUTFILE2}"
    mkdir -p "${RESULT_DIR}"

    echo "=== Submitting P=1 baseline jobs ==="
    for N in $N_VALUES; do
        submit_job "par_pi"        $N 1 1 1 "a" "$OUTFILE1"
        submit_job "par_pi_simple" $N 1 1 1 "a" "$OUTFILE2"
    done
    echo ""
    echo "Wait for these to finish, then run: ./run_all.sh rest"
}

rest() {
    echo "=== Combination (a): p1=ceil(P/32), p2=ceil(P/p1) ==="
    for N in $N_VALUES; do
        submit_job "par_pi"        $N 8   1 8  "a" "$OUTFILE1"
        submit_job "par_pi_simple" $N 8   1 8  "a" "$OUTFILE2"

        submit_job "par_pi"        $N 32  1 32 "a" "$OUTFILE1"
        submit_job "par_pi_simple" $N 32  1 32 "a" "$OUTFILE2"

        submit_job "par_pi"        $N 128 4 32 "a" "$OUTFILE1"
        submit_job "par_pi_simple" $N 128 4 32 "a" "$OUTFILE2"

        submit_job "par_pi"        $N 256 8 32 "a" "$OUTFILE1"
        submit_job "par_pi_simple" $N 256 8 32 "a" "$OUTFILE2"
    done

    echo ""
    echo "=== Combination (b): p1=min(P,9), p2=ceil(P/p1) ==="
    for N in $N_VALUES; do
        submit_job "par_pi"        $N 8   8 1  "b" "$OUTFILE1"
        submit_job "par_pi_simple" $N 8   8 1  "b" "$OUTFILE2"

        submit_job "par_pi"        $N 32  9 4  "b" "$OUTFILE1"
        submit_job "par_pi_simple" $N 32  9 4  "b" "$OUTFILE2"

        submit_job "par_pi"        $N 128 9 15 "b" "$OUTFILE1"
        submit_job "par_pi_simple" $N 128 9 15 "b" "$OUTFILE2"

        submit_job "par_pi"        $N 256 9 29 "b" "$OUTFILE1"
        submit_job "par_pi_simple" $N 256 9 29 "b" "$OUTFILE2"
    done

    echo ""
    echo "All jobs submitted! Check with: squeue -u \$USER"
    echo "After all jobs finish, run: ./run_all.sh collect"
}

collect() {
    : > "${OUTFILE1}"
    : > "${OUTFILE2}"

    for f in ${RESULT_DIR}/par_pi_n*_*.txt; do
        [ -e "$f" ] || continue
        cat "$f" >> "${OUTFILE1}"
        echo "" >> "${OUTFILE1}"
    done

    for f in ${RESULT_DIR}/par_pi_simple_n*_*.txt; do
        [ -e "$f" ] || continue
        cat "$f" >> "${OUTFILE2}"
        echo "" >> "${OUTFILE2}"
    done

    echo "Collected results into:"
    echo "  ${OUTFILE1}"
    echo "  ${OUTFILE2}"
}

case "$1" in
    compile)  compile ;;
    baseline) baseline ;;
    rest)     rest ;;
    collect)  collect ;;
    *)
        echo "Usage: ./run_all.sh {compile|baseline|rest|collect}"
        echo "  Step 1: ./run_all.sh compile"
        echo "  Step 2: (等编译完) ./run_all.sh baseline"
        echo "  Step 3: (等P=1完) ./run_all.sh rest"
        echo "  Step 4: (等所有任务结束) ./run_all.sh collect"
        ;;
esac
