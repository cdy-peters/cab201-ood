NC='\033[0m'
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'

test()
{
    for color in $(ls cases/$grade); do
        if [ ! -d cases/$grade/$color/output ]; then
            mkdir cases/$grade/$color/output
        fi

        for i in $(ls cases/$grade/$color/tests); do
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
        done
    done
}

if [ $# -eq 0 ]; then
    for grade in $(ls cases); do
        test
    done
else
    grade=$1
    test
fi