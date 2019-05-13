package group;

import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;
import javax.swing.JLabel;

public class Fibonacci implements Runnable
{
	// FIELDS
	private JLabel label;
	private int delay;
	
	// CONSTRUCTOR
	public Fibonacci(JLabel result, int delay)
	{
		this.label = result;
	    this.delay = delay;
	}

	// METHODS
	public void run()
	{
	   Lock fibonacciLock = new ReentrantLock();
	   fibonacciLock.lock();
	   try
	   {
		    long prev1 = 0;
   			long prev2 = 1;
   			long next = 0;
   			while(next<Long.MAX_VALUE)
   			{
   				next = prev1+prev2;
   				prev1 = prev2;
   				prev2 = next;
   				label.setText(Long.toString(next));
   				Thread.sleep(delay);
   			}
	   }
	   catch (InterruptedException e)
	   {       
		   System.err.println(e.getMessage());
	   }
	   finally
	   {
		   fibonacciLock.unlock();
	   }
	}
}
