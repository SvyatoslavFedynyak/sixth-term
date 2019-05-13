package group;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Paint;
import java.awt.Shape;
import java.awt.Stroke;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.geom.AffineTransform;
import java.awt.geom.GeneralPath;
import java.awt.geom.Line2D;
import java.awt.geom.Point2D;
import java.util.Random;
import java.util.Vector;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

import javax.swing.JPanel;

public class Epicycloid extends JPanel implements Runnable
{
	// FIELDS
	private Random random;
	private Paint paint;
	private Stroke stroke;
	private double step = 0.01D;
    private Vector<Point2D> points = new Vector<Point2D>();
    private int delay;

	// CONSTRUCTOR
	public Epicycloid(int delay)
	{
		paint = Color.red;
		stroke = new BasicStroke(1);
		random = new Random();
		this.delay = delay;
		addMouseListener(new MouseAdapter()
		{
			public void mousePressed(MouseEvent event)
			{
				// random color
				paint = new Color(random.nextFloat(), random.nextFloat(), random.nextFloat());

				// random stroke
				int width = random.nextInt(15);
				int cap = random.nextInt(3);
				int join = random.nextInt(3);
				stroke = new BasicStroke(width, cap, join);

				repaint();
			}
		});
	}

	// METHODS
	public void paintComponent(Graphics g)
	{
		super.paintComponent(g);

		g.setColor(Color.WHITE);
		g.fillRect(0, 0, getWidth(), getHeight());
		Graphics2D g2 = (Graphics2D) g;
		g2.setPaint(Color.black);

		Shape xAxis = new Line2D.Double(0, getHeight() / 2, getWidth(), getHeight() / 2);
		Shape yAxis = new Line2D.Double(getWidth() / 2, 0, getWidth() / 2, getHeight());
		g2.draw(xAxis);
		g2.draw(yAxis);

		// transform to center to correct math axes
		AffineTransform oldTransform = g2.getTransform();
		g2.translate(getWidth()/2, getHeight()/2);

		//Set random values
		g2.setPaint(paint);
		g2.setStroke(stroke);

		if (points.size() != 0)
		{
			GeneralPath path = new GeneralPath();
			path.moveTo(points.firstElement().getX(), points.firstElement().getY());
			for (Point2D p : points)
			{
				path.lineTo(p.getX(), p.getY());
			}
			path.closePath();
			g2.draw(path);
		}

		// reset transform
		g2.setTransform(oldTransform);
	}
	
	public void run()
	{
		Lock epicycloidLock = new ReentrantLock();
		epicycloidLock.lock();
		try
		{
			int a = getWidth() / 30;
			int A = getWidth() / 15;
	    	for(double t = -Math.PI; t <= Math.PI; t += step)
	    	{
				double x = (A + a)*Math.cos(t)-a*Math.cos((A/a+1)*t);
				double y = (A + a)*Math.sin(t)-a*Math.sin((A/a+1)*t);

	    		points.add(new Point2D.Double(x,y));
	    		repaint();
				Thread.sleep(delay);
	    	}
		} 
		catch (InterruptedException e) 
		{
			e.printStackTrace();
		}
		finally
		{
			epicycloidLock.unlock();
		}
	}
}
