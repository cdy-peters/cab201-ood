NC='\033[0m'
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'

run_tests()
{
    if [ ! -d cases/$grade/$color/output ]; then
        mkdir cases/$grade/$color/output
    fi

    for i in $(ls cases/$grade/$color/tests | sort -n); do
        test
    done
}

test()
{
    echo "${NC}__________________________________________________"
    echo "Testing $grade/$color/$i\n"
    ./../bin/Debug/net7.0/CAB201 $color cases/$grade/$color/tests/$i cases/$grade/$color/output/$i

    if [ ! -f cases/$grade/$color/output/$i ]; then
        echo "${YELLOW}error"
        continue
    fi

    diff -b -s cases/$grade/$color/output/$i cases/$grade/$color/answers/$i
    if [ $? -eq 1 ]; then
        echo "${RED}false"
    else
        echo "${GREEN}true"
    fi
}

if [ $# -eq 0 ]; then
    for grade in $(ls cases); do
        for color in $(ls cases/$grade | sort -n); do
            run_tests
        done
    done
elif [ $# -eq 1 ]; then
    grade=$1
    for color in $(ls cases/$grade | sort -n); do
        run_tests
    done
elif [ $# -eq 2 ]; then
    grade=$1
    color=$2
    run_tests
elif [ $# -eq 3 ]; then
    grade=$1
    color=$2
    i=$3.txt
    test
else
    echo "Usage: ./test.sh [grade] [color] [test]"
fi