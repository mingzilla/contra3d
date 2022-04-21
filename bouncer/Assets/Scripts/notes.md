## Referencing objects

### Add rigidbody using code
```cs
void Start() {
  Rigidbody rb = gameObject.AddComponent<Rigidbody>();
  rb.useGravity = false;
}
```

### FindObjectOfType<ComponentNameItHas> - slow

### FindWithTag - faster