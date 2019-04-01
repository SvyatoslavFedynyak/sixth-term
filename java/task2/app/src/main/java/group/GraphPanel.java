package group;

import java.awt.*;
import javax.swing.*;
import java.awt.geom.*;
import java.util.Random;

public class GraphPanel extends JPanel
{
    // FIELDS
    Graphs graphs;

    Random random;
    Paint paint;
    Stroke stroke;
    Stroke axesStroke;

    // CONSTRUCTORS
    public GraphPanel()
    {
        graphs = new Graphs();

        random = new Random();
        paint = Color.red;
        stroke = new BasicStroke(1);
        axesStroke = new BasicStroke(1);

        // change stroke randomly on click
        addMouseListener(new java.awt.event.MouseAdapter()
        {
             public void mousePressed(java.awt.event.MouseEvent event)
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

        // get Graphics2D
        Graphics2D g2d = (Graphics2D)g;

        // drawing
        drawGraph(g2d);
        drawAxes(g2d);
        drawName(g2d);
    }
    private void drawAxes(Graphics2D g2d)
    {
        g2d.setPaint(Color.black);
        g2d.setStroke(axesStroke);
        Shape xAxis = new Line2D.Float(0, getHeight() / 2, getWidth(), getHeight() / 2);
        Shape yAxis = new Line2D.Float(getWidth() / 2, 0, getWidth() / 2, getHeight());
        g2d.draw(xAxis);
        g2d.draw(yAxis);
    }
    private void drawName(Graphics2D g2d)
    {
        g2d.setColor(Color.black);
        g2d.setFont(new Font("SansSerif", Font.PLAIN, 15));
        g2d.drawString("Fedynyak Svyatoslav, v21 -> v8", 10, 20);
    }
    private void drawGraph(Graphics2D g2d)
    {
        // sets alpha, to scale graph
        graphs.setA(getWidth()/5);

        // transform to center and rotate axes to correct math axes
        AffineTransform oldTransform = g2d.getTransform();
        g2d.translate(getWidth()/2, getHeight()/2);
        g2d.rotate(Math.toRadians(-90));

        // sets random color and stroke
        g2d.setPaint(paint);
        g2d.setStroke(stroke);

        // drawing graphs
        GeneralPath graphPath = new GeneralPath();
        graphPath.moveTo( 0,  0 );
        for (float phi = 0; phi <= 2 * Math.PI; phi += 0.01F)
        {
            Point2D point = graphs.FoliumOfDescartes(phi);
            graphPath.lineTo(point.getX(), point.getY());
        }
        graphPath.closePath();
        g2d.draw(graphPath);

        // reset transform
        g2d.setTransform(oldTransform);
    }
}