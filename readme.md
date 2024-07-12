# PEzBus

[![forthebadge](http://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)  [![forthebadge](http://forthebadge.com/images/badges/powered-by-electricity.svg)](http://forthebadge.com)

Certaines applications traitant de grands flux d'entrées/sorties de données (i.e les OMS, serveurs de jeux videos ...) nécessitent une grande organisation pour orchestrer l'impacte d'une nouvelle donnée ou action sur les données existantes. Dans ce cas précis, développer un système drivé par les évènements permet de créer un effets dominos et mettre un nom sur ces changements.

Le java a son package Guava de google, qui offre un bus d'évènement et implémente le pattern Publish / Subscribe et le rend très accessible grâce aux décorateurs de méthodes :

```
...
MyEvent event = new MyEvent();
eventBus.post(event);
@Subscribe
public void handleEvent(MyEvent event){
    ...
}

```

Ce mode de fonctionnement rend l'utilisation des évènements plus convenable et de surcroit plus simple.
L'idée de ce package et d'apporter ce mode de fonctionnement plus adéquat aux framework .NET

Par expérience, ce qui manque à cette fonctionalité est la capabilité de modéliser l'impacte d'un évènement sur le système. Pouvoir générer une documentation qui se base sur les évènements et les commentaires XML pour modéliser un système est à mon sens largement suffisant pour décrire le fonctionnement d'un système basé sur les évènements.

### Performance

Comme les performances sont fondamentalement importantes, voici des tests realisés : 

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3

| Method        | N       | Mean          | Error        | StdDev      | Allocated    |
|-------------- |-------- |--------------:|-------------:|------------:|-------------:|
| PublishEvents | 1       |      1.699 us |     3.426 us |   0.1878 us |      2.05 KB |
| PublishEvents | 1000    |     73.104 us |    13.348 us |   0.7316 us |    316.51 KB |
| PublishEvents | 4000    |    251.948 us |    39.923 us |   2.1883 us |   1254.06 KB |
| PublishEvents | 12000   |    704.147 us |   411.158 us |  22.5370 us |   3754.17 KB |
| PublishEvents | 25000   |  1,429.166 us |   278.583 us |  15.2701 us |   7816.96 KB |
| PublishEvents | 100000  |  5,655.089 us | 1,939.056 us | 106.2862 us |  31254.84 KB |
| PublishEvents | 1000000 | 55,356.963 us | 6,016.712 us | 329.7963 us | 312504.88 KB |


### Pré-requis

Principalement du café et optionellement un clavier

### Installation

```

 dotnet add package PEzBus

```

## Démarrage

L'utilisation est plutôt simple : 
- Créer des classes représentants des évènements (accessoirement avec des attributs pour la transmission de donnée) qui implémente l'interface IEzEvent. L'idéale est de choisir des noms claires qui définissent l'évènement en question : i.e OrderCreatedEvent
- Implémenter des méthodes décorées avec l'attribut Subscribe comme suit : 
    ```
    public class LaClass {
        [Subscribe(typeof(MonSuperEvent))]
        public void MaSuperMethode(){
            ...
        }
    }
    ```
    ou encore : 

     ```
    public class LaClass {
        [Subscribe(typeof(MonSuperEvent))]
        public void MaSuperMethode(MonSuperEvent event){
            var eventArg = event.GetEventArg();
            eventArg.TransformationSayen();
        }
    }
    ```
- Créer une instance du bus d'évènement : 
```
EventBus eventBus = new();
```
- Inscrire une instance de la class dans ce bus d'évènement :
```
var maClass = new LaClass();
eventBus.Register<LaClass>(maClass);
```
- Publier l'évènement qui se produit : 
```
var event = new MonSuperEvent(...params);
eventBus.Publish(event);
```

Ensuite,les méthodes ayant comme paramètre de l'attribut Subscribe le type MonSuperEvent ```[Subscribe(typeof(MonSuperEvent))]``` seront invokés, si et seulement si une instance de classe contenant la méthode est inscrite dans le bus d'évènement.




## Versions

**Première version :** 1.0.1
Implémentation du bus d'évènements qui constitue le coeur du projet

## TODO : 
Modéliser l'impacte que pourrait avoir un évènement sur les données existantes dans l'application en se basant sur le nom des évènements, le nom des méthodes, ainsi que les commentaires XML.

## Auteurs

* **Aymane EL JAHRANI** _alias_ [@eljaymane](https://github.com/eljaymane)


## License


