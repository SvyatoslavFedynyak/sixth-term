package group;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;
import javax.swing.JPanel;

public class FloatText extends JPanel implements Runnable
{
	// FIELDS
	private int x;
	private int y;
	private int delay;

	// CONSTRUCTOR
	public FloatText(int delay)
	{
		x = 0;
		y = 50;
		this.delay = delay;
	}

	// METHODS
	public void paintComponent(Graphics g)
	{
		super.paintComponent(g);
	    g.setColor(Color.WHITE);
	    g.fillRect(0, 0, getWidth(), getHeight());
		Font font = new Font("SansSerif", Font.PLAIN, 15);
	    g.setColor(Color.black);
	    g.setFont(font);
	    g.drawString("Floating text",x,y);
	}
	
	public void run()
	{
		Lock floatTextLock = new ReentrantLock();
		floatTextLock.lock();
		try
		{
			for(long i=0; i<Long.MAX_VALUE; ++i)
			{
				x=(++x)%getWidth();
				repaint();
				Thread.sleep(delay);
			}
		}
		catch(InterruptedException e)
		{
			System.err.println(e.getMessage());
		}
		finally
		{
			floatTextLock.unlock();
		}
	}
}
