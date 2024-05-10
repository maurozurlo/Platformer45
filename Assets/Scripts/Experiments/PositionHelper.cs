using UnityEngine;

public class PositionHelper : MonoBehaviour
{
    // Function to determine which axis has changed between two positions
    public static char GetModifiedAxis(Vector3 newPosition)
    {
        if (newPosition.x != 0f)
        {
            return 'x';
        }
        else if (newPosition.y != 0f)
        {
            return 'y';
        }
        else if (newPosition.z != 0f)
        {
            return 'z';
        }
        else
        {
            // No axis has changed
            return '\0';
        }
    }

    // Function to create a new Vector3 with the same data as startPosition except for the modified axis
    public static Vector3 CreateVectorWithModifiedAxis(Vector3 startPosition, Vector3 newPosition, char modifiedAxis)
    {
        Vector3 modifiedVector = new Vector3(startPosition.x, startPosition.y, startPosition.z);

        switch (modifiedAxis)
        {
            case 'x':
                modifiedVector.x = newPosition.x;
                break;
            case 'y':
                modifiedVector.y = newPosition.y;
                break;
            case 'z':
                modifiedVector.z = newPosition.z;
                break;
            default:
                // No axis has changed, return original position
                modifiedVector = startPosition;
                break;
        }

        return modifiedVector;
    }

    // Function to determine which axis has the biggest value in a Vector3
    public static char GetAxisWithLargestValue(Vector3 vector)
    {
        // Initialize variables to store the largest value and its corresponding axis
        float largestValue = Mathf.NegativeInfinity;
        char largestAxis = '\0';

        // Compare the values of each axis
        if (vector.x > largestValue)
        {
            largestValue = vector.x;
            largestAxis = 'x';
        }
        if (vector.y > largestValue)
        {
            largestValue = vector.y;
            largestAxis = 'y';
        }
        if (vector.z > largestValue)
        {
            largestAxis = 'z';
        }

        return largestAxis;
    }
}