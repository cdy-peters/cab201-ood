
test()
{
    for color in $(ls cases/$grade); do
        if [ ! -d cases/$grade/$color/output ]; then
            mkdir cases/$grade/$color/output
        fi

        for i in $(ls cases/$grade/$color/tests); do
            ./../bin/Debug/net7.0/CAB201 $color cases/$grade/$color/tests/$i cases/$grade/$color/output/$i
            cmp cases/$grade/$color/output/$i cases/$grade/$color/answers/$i
            if [ $? -eq 1 ]; then
                echo "false"
            else
                echo "true"
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