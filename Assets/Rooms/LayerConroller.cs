using UnityEngine;

public class LayerConroller : MonoBehaviour {
    public Menu DungeonController;

    private void Start() {
        DungeonController = GameObject.FindObjectOfType<Menu>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            DungeonController.OpenLevelMenu();
        }
    }
}
