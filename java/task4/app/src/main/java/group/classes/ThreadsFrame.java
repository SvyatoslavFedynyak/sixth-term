package group;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class ThreadsFrame extends JFrame implements ActionListener
{
    // FIELDS
    private JLabel label1 = new JLabel("Потік 1 - графік:");
    private JButton start1 = new JButton("Старт");
    private JButton pause1 = new JButton("Стоп");
    private JButton resume1 = new JButton("Продовжити");
    
    private JLabel label2 = new JLabel("Потік 2 - числа фібоначі:");
    private JButton start2 = new JButton("Старт");
    private JButton pause2 = new JButton("Стоп");
    private JButton resume2 = new JButton("Продовжити");
    private JLabel result2 = new JLabel("Ресурс 2-го потоку");
    
    private JLabel label3 = new JLabel("Потік 3 - плаваючий текст:");
    private JButton start3 = new JButton("Старт");
    private JButton pause3 = new JButton("Стоп");
    private JButton resume3 = new JButton("Продовжити");
    
    private JPanel mainPanel = new JPanel(new GridLayout(3,1,10,10));
    private JPanel panel1 = new JPanel(new BorderLayout(10, 10));
    private JPanel panel2 = new JPanel(new BorderLayout(10, 10));
    private JPanel panel3 = new JPanel(new BorderLayout(10, 10));
    private Thread[] threads = new Thread[3];
    private Epicycloid epicycloid;
    private Fibonacci fibonacci;
    private FloatText floatText;

    // CONSTRUCTOR
    public ThreadsFrame()
    {
        super("Java threads ");
        setSize(new Dimension(700, 800));
        setResizable(false);
        setLayout(new BorderLayout(10, 10));

        JPanel buttons1 = new JPanel(new GridLayout(1,3,10,10));
        label1.setForeground(Color.RED);
        start1.addActionListener(this);
        pause1.addActionListener(this);
        resume1.addActionListener(this);
        buttons1.add(label1);
        buttons1.add(start1);
        buttons1.add(pause1);
        buttons1.add(resume1);
        panel1.add(buttons1, BorderLayout.NORTH);
        epicycloid = new Epicycloid(50);
        panel1.add(epicycloid, BorderLayout.CENTER);
        panel1.setBorder(BorderFactory.createMatteBorder(0, 0, 1, 0, Color.BLACK));
        
        JPanel buttons2 = new JPanel(new GridLayout(1,3,10,10));
        label2.setForeground(Color.RED);
        start2.addActionListener(this);
        pause2.addActionListener(this);
        resume2.addActionListener(this);
        buttons2.add(label2);
        buttons2.add(start2);
        buttons2.add(pause2);
        buttons2.add(resume2);
        panel2.add(buttons2, BorderLayout.NORTH);
        fibonacci = new Fibonacci(result2,1000);
        panel2.add(result2, BorderLayout.CENTER);
        panel2.setBorder(BorderFactory.createMatteBorder(0, 0, 1, 0, Color.BLACK));
        
        JPanel buttons3 = new JPanel(new GridLayout(1,3,10,10));
        label3.setForeground(Color.RED);
        start3.addActionListener(this);
        pause3.addActionListener(this);
        resume3.addActionListener(this);
        buttons3.add(label3);
        buttons3.add(start3);
        buttons3.add(pause3);
        buttons3.add(resume3);
        panel3.add(buttons3, BorderLayout.NORTH);
        floatText = new FloatText(20);
        panel3.add(floatText);

        mainPanel.add(panel1);
        mainPanel.add(panel2);
        mainPanel.add(panel3);
        mainPanel.setBorder(BorderFactory.createEmptyBorder(10, 10, 10, 10));
        add(mainPanel);
    }

    // METHODS
    public void actionPerformed(ActionEvent e)
    {
        if(e.getSource()==start1)
        {
        	threads[0] = new Thread(epicycloid);
        	threads[0].start();
        }
        else if(e.getSource()==start2)
        {
        	threads[1] = new Thread(fibonacci);
        	threads[1].start();
        }
        else if(e.getSource()==start3)
        {
        	threads[2] = new Thread(floatText);
        	threads[2].start();
        }
        else if(e.getSource()==pause1)
        {
        	threads[0].suspend();
        }
        else if(e.getSource()==pause2)
        {
			threads[1].suspend();
        }
        else if(e.getSource()==pause3)
        {
        	threads[2].suspend();
        }
        else if(e.getSource()==resume1)
        {
        	threads[0].resume();
        }
        else if(e.getSource()==resume2)
        {
        	threads[1].resume();
        }
        else if(e.getSource()==resume3)
        {
        	threads[2].resume();
        }
    } 
}
