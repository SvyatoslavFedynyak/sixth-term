package main;

public class Main {

    public static void main(String[] args) {
        Task1.doTask();

        Task2.doTask("rat,tomatto,vertex,kitticat");
        System.out.println();
        EducationalInstitution [] arr = new EducationalInstitution[6];
        arr[0] = new School("lpml", "lubinska", 1993, 23, 442);
        arr[1] = new University("lnu1", "universytetska1", 1997, 4, 12);
        arr[2] = new University("lnu2", "universytetska2", 1993, 3, 18);
        arr[3] = new School("lpml2", "lubinska", 1995, 12, 357);
        arr[4] = new University("lnu3", "universytetska3", 1998, 2, 15);
        arr[5] = new School("lpml3", "lubinska", 1996, 23, 189);
        arr[4] = new University("lnu4", "universytetska4", 1998, 3, 15);
        Task3.doTask(arr);

    }
}
