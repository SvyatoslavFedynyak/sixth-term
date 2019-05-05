package group;

public class Graphs
{
    // FIELDS
    float a;

    // CONSTRUCTORS
    public Graphs()
    {
        this(50);
    }
    public Graphs(float a)
    {
        this.a = a;
    }

    // METHODS
    public void setA(float a)
    {
        this.a = a;
    }

    // GRAPHICS
    public java.awt.geom.Point2D FoliumOfDescartes(double phi)
    {
        double t = Math.tan(phi);
        double _1_plus_t_pow3 =  1 + Math.pow(t, 3);
        double _3_mul_a_mul_t = 3*a*t;

        // calc (x, y)
        return new java.awt.geom.Point2D.Double(
                (_3_mul_a_mul_t)/_1_plus_t_pow3,
                (_3_mul_a_mul_t*t)/_1_plus_t_pow3
        );
    }
}