package main;

import java.util.Scanner;

public class Task1 {
    Task1() {}
    
    public static int numberSum(int number) {
        int sum = 0;
        while (number != 0) {
            sum += (number % 10);
            number /= 10;
        }
        return sum;
    }

    public static int max(int[] arr) {
        int max = arr[0];
        for (int item : arr) {
            if (item > max) {
                max = item;
            }
        }
        return max;
    }

    public static void doTask(){
        Scanner sc = new Scanner(System.in);
        System.out.println("task 1");
        System.out.println("enter n and m:");
        int n, m;
        n = sc.nextInt();
        m = sc.nextInt();
        int [] vec = new int [n];
        int [][] arr = new int[n][m];
        for (int i = 0; i < n; i++){
            vec[i] = 11;
        }
        for (int i = 0; i < n; i++){
            for (int j = 0; j < m; j++){
                arr[i][j] = (int)(Math.random()*10);
            }
        }

        for (int i = 0; i < n; i++){
            for (int j = 0; j < m; j++){
                System.out.print(arr[i][j] + " ");
            }
            System.out.println();
        }

        int[] temp = new int[m];
        for (int i = 0; i < n; i++){
            for (int j = 0; j < m; j++) {
                temp[j] = numberSum(arr[i][j]);
            }
            vec[i] = max(temp);
        }
        System.out.println();
        for (int i = 0; i < n; i++){
            System.out.print(vec[i] + " ");
        }
        System.out.println();
        System.out.println();
    }
}
