# TD flowchart

## flowchart for enemy pathfinding
```mermaid
flowchart TD
    Start((Start)) --> A(spawn enemies)
    A --> B{kan enemy naar basis?}
    B --> | ja | C(Ga naar basis)
    B --> | nee | D(zoek dichtsbijzijnde building op het pad naar de basis)
    D --> E(sloop building)
    E --> B
    C --> F{word enemy beschoten?}
    F --> | ja | G(ga naar schutter) --> I(sloop schutter)
    I --> F
    F --> | nee | H(deal damage aan basis)
    H --> J{basis nog heel?}
    J --> | nee | End((End))
    J --> | ja | Start
 ```
