package group;

import java.awt.*;

public class MainFrame extends javax.swing.JFrame
{
    // CONST
    public static final int DEFAULT_WIDTH = 400;
    public static final int DEFAULT_HEIGHT = 400;

    // CONSTRUCTORS
    public MainFrame()
    {
        // sets title and size
        setTitle("Graph");
        setSize(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        setMinimumSize(new Dimension(DEFAULT_WIDTH, DEFAULT_HEIGHT));

        // get screen dimensions
        Dimension screenSize = Toolkit.getDefaultToolkit().getScreenSize();

        // center frame in screen
        setLocation(screenSize.width/2 - DEFAULT_WIDTH/2, screenSize.height/2 - DEFAULT_HEIGHT/2);

        // add panel to frame
        add(new GraphPanel ());
    }
}