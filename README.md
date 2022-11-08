# Apocalypse Tower Defense

## Gameplay
1. Maak een nieuwe save.
2. Kies een level en difficulty.
3. Als je op een level drukt en op play laad het level en kan je jouw base plaatsen.
4. Je kan rechts de shop, je resources en de start wave knop zien.
5. Plaats de torens die je nodig denkt te hebben.
6. Als je op start wave drukt zullen er zombies bij alle ingangen spawnen.
7. Blijf muren en torens bouwen totdat je alle zombies hebt gedood of hebt verloren.

## Technisch
Ik had de meeste tijd besteed aan de zombie pathfinding wat door veel verschillende versies is gegaan.

De eerste versie was zonder hulp van een turorial een gridbased pathfinding te maken maar er wareb nogal wat bugs met het detecten van boundaries waardoor het niet performant was.

De tweede versie was deze tutorial: https://www.youtube.com/watch?v=tSe6ZqDKB0Y&list=PLSft82iKx-nX2G7CwuDwbiH2sagiDFXJH&index=7&t=1156s. Nadat ik de tutorial meerdere keren heb gevolgd wist ik nog steeds niet hoe flowmaps precies werken en dus kon ik er ook vrij weinig aan veranderen zodat het voor deze tower defense werkte.

de derde en laatste versie is de A* tutorial van Sebastian Lague op youtube: https://www.youtube.com/c/SebastianLague hij is heel erg goed in het uitleggen van zijn onderwerpen. Ik heb na het volgen van de tutorial veel dingen in de code veranderd zoals: hoe zombies weten waar ze wel en niet mogen lopen en er voor gezorgd dat de zombies van pad kunnen veranderen.
