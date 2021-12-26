namespace RabbitMQ_v6.Models;

public class TTC
{
    public HitRange Range;

    public int ShootingRange { get; set; }

    public bool HasClip { get; set; }
    
    public bool HasOptics { get; set; }
}