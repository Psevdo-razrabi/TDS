using System.Collections.Generic;

public class AISensors
{
    private List<IAISensor> _sensors = new List<IAISensor>();
    private IAISensor _lastTriggeredSensor = null;

    public void AddSensor(IAISensor sensor)
    {
        _sensors.Add(sensor);
    }

    public void AddSensor(IAISensor sensor1, IAISensor sensor2)
    {
        _sensors.Add(sensor1);
        _sensors.Add(sensor2);
    }

    public (bool, IAISensor) IsTriggeredFirst()
    {
        if (_sensors.Count < 1)
        {
            return (false, null);
        }

        foreach (IAISensor sensor in _sensors)
        {
            if (sensor.IsTriggered())
            {
                _lastTriggeredSensor = sensor;
                return (true, sensor);
            }
        }

        return (false, null);
    }

    public IAISensor GetLastTriggereSensor()
    {
        return _lastTriggeredSensor;
    }

}
