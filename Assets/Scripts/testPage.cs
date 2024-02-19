using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class testPage : MonoBehaviour {
    Label angleText;
    // Start is called before the first frame update
    void Start() {
        AngleTracker.OnAngleCalculated += AngleTracker_OnAngleCalculated;

        MiniPage miniPage = new MiniPage(this);
        angleText = miniPage.CreateAndAddElement<Label>();
        angleText.style.fontSize = 50;
    }

    private void AngleTracker_OnAngleCalculated(object sender, AngleTracker.OnAngleCalculatedEventArgs e) {
        angleText.text = e.angle.ToString();
    }
}
