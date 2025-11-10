namespace SoundMaster;

public record EnvelopePoint(double T, double A);

public enum AdsrEnvelopeName
{
    Attack,
    Decay,
    Sustain,
    Release
};

public class Sound : RawSound, ICloneable
{
    public double Frequency { get; private set; }
    public short Volume { get; private set; }
    public EnvelopePoint[] AdsrEnvelope { get;  private set; }

    public Sound(int sampleRate = 44100, double frequency = 440.0, double duration = 1.0,
        short volume = short.MaxValue): base()
    {
        SampleRate = sampleRate;
        Frequency = frequency;
        Duration = duration;
        Volume = volume;
        AdsrEnvelope = DefaultAdsrEnvelope();
    }

    public object Clone() => MemberwiseClone();

    public Sound GetSample(double? f = null, double? d = null)
    {
        var frequency = f ?? this.Frequency;
        var duration = d ?? this.Duration;
        var frames = (int) (SampleRate * duration);
        
        Buffer = new short[frames];
        for (var i = 0; i < frames; i++)
        {
            Buffer[i] = (short)(Volume * GetAdsrValue((double)(i)/frames) * Math.Sin(2 * Math.PI * frequency * i / SampleRate));
        }

        return this;
    }

    public double[] SimulateEnvelope()
    {
        var res = new double[30];
        for (var i = 0; i < 30; i++)
            res[i] = GetAdsrValue((double)(i)/30);
        return res;
    }

    private double GetAdsrValue(double t)
    {
        if (t < AdsrEnvelope[(int)AdsrEnvelopeName.Attack].T)
            return 0;
        if (t < AdsrEnvelope[(int)AdsrEnvelopeName.Decay].T)
            return AttackPhase(t);
        if (t < AdsrEnvelope[(int)AdsrEnvelopeName.Sustain].T)
            return DecayPhase(t);
        if (t < AdsrEnvelope[(int)AdsrEnvelopeName.Release].T)
            return SustainPhase(t);
        return ReleasePhase(t);
    }
    
    private double AttackPhase(double t) => 
        t * (AdsrEnvelope[1].A - AdsrEnvelope[0].A)/(AdsrEnvelope[1].T - AdsrEnvelope[0].T) + AdsrEnvelope[0].A;
    
    private double DecayPhase(double t) => 
        (t - AdsrEnvelope[1].T) * (AdsrEnvelope[2].A - AdsrEnvelope[1].A)/(AdsrEnvelope[2].T - AdsrEnvelope[1].T) + AdsrEnvelope[1].A;
    
    private double SustainPhase(double t) => 
        (t - AdsrEnvelope[2].T) * (AdsrEnvelope[3].A - AdsrEnvelope[2].A)/(AdsrEnvelope[3].T - AdsrEnvelope[2].T) + AdsrEnvelope[2].A;
    
    private double ReleasePhase(double t) => 
        (t - AdsrEnvelope[3].T) * (0.0 - AdsrEnvelope[3].A)/(1.0 - AdsrEnvelope[3].T) + AdsrEnvelope[3].A;

    public static bool ValidateAdsrEnvelopePoint(EnvelopePoint point, bool throwError = true)
    {
        if ((point.T is >= 0 and <= 1) && (point.A is >= 0 and <= 1))
        {
            return true;
        }
        else if(throwError)
        {
            throw new ArgumentException("Adsr point values must be between 0 and 1", nameof(point));
        }
        return false;
    }
    
    public void SetAdsrEnvelope(AdsrEnvelopeName name, EnvelopePoint point)
    {
        ValidateAdsrEnvelopePoint(point);
        
        if(name != AdsrEnvelopeName.Release && AdsrEnvelope[(int)name + 1].T < point.T)
           throw new ArgumentException("Adsr point's time exceeds next next stamp", nameof(name)); 
        
        AdsrEnvelope[(int)name] = point; 
    }

    public void SetAdsrEnvelope(EnvelopePoint[] adsrEnvelope)
    {
       if(adsrEnvelope.Length != 4) 
           throw new ArgumentOutOfRangeException(nameof(adsrEnvelope), "Adsr must be of size 4");

       for (var i = 1; i < adsrEnvelope.Length; i++)
       {
           if(adsrEnvelope[i].T < adsrEnvelope[i - 1].T)
               throw new ArgumentException("Adsr point's time exceeds next next stamp", nameof(adsrEnvelope));
           ValidateAdsrEnvelopePoint(adsrEnvelope[i]);
       }
       
       AdsrEnvelope = adsrEnvelope;
    }
    
    private static EnvelopePoint[] DefaultAdsrEnvelope()
    {
        return
        [
            new EnvelopePoint(0, 0),
            new EnvelopePoint(0.17, 1),
            new EnvelopePoint(0.4, 0.7),
            new EnvelopePoint(0.8, 0.7)
        ];
    }
}