using UnityEngine;

public enum CurrentVersion
{
    PC,
    PHONE
}

public class InputManager : MonoBehaviour
{
    public CurrentVersion CurrentVersion;

    [SerializeField] ZoneButton leftZone;
    [SerializeField] ZoneButton rightZone;

    void Update()
    {
        if (SpaceshipController.Instance == null) return;

        bool isLeftPressed = false;
        bool isRightPressed = false;

        switch (CurrentVersion)
        {
            case CurrentVersion.PC:
                isLeftPressed = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
                isRightPressed = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
                break;

            case CurrentVersion.PHONE:
                isLeftPressed = leftZone != null && leftZone.IsPressed;
                isRightPressed = rightZone != null && rightZone.IsPressed;
                break;
        }

        if (isLeftPressed && isRightPressed)
        {
            SpaceshipController.Instance.Shoot();
        }
        else if (isLeftPressed)
        {
            SpaceshipController.Instance.Move(-1f);
        }
        else if (isRightPressed)
        {
            SpaceshipController.Instance.Move(1f);
        }
    }
}