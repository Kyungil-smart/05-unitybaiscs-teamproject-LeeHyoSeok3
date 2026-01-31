using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Shuffle
{
    public static void Array<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            // 0 ~ i 사이의 랜덤 인덱스 생성
            int randomIndex = Random.Range(0, i + 1);

            // 요소 교환 (Swap)
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
