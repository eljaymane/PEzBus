# PEzBus

[![forthebadge](http://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)  [![forthebadge](http://forthebadge.com/images/badges/powered-by-electricity.svg)](http://forthebadge.com)

Certaines applications traitant de grands flux d'entrées/sorties de données (i.e les OMS, serveurs de jeux videos ...) nécessitent une grande organisation pour orchestrer l'impacte d'une nouvelle donnée ou action sur les données existantes. Dans ce cas précis, développer un système drivé par les évènements permet de créer un effets dominos et mettre un nom sur ces changements.

Le java a son package Guava de google, qui offre un bus d'évènement et implémente le pattern Publish / Subscribe et le rend très accessible grâce aux décorateurs de méthodes :

```java {"id":"01J2KXAXNVT7KR99R7Z2Q6EGFE"}
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

| Method        | N       | Mean          | Error         | StdDev      | Allocated    |
|-------------- |-------- |--------------:|--------------:|------------:|-------------:|
| PublishEvents | 1       |      1.060 us |     0.0868 us |   0.0048 us |      2.05 KB |
| PublishEvents | 1000    |     37.127 us |     5.4560 us |   0.2991 us |    316.54 KB |
| PublishEvents | 4000    |    139.452 us |    31.1420 us |   1.7070 us |   1254.08 KB |
| PublishEvents | 12000   |    500.748 us |   442.2057 us |  24.2388 us |   3753.92 KB |
| PublishEvents | 25000   |    824.892 us |   653.0087 us |  35.7936 us |   7816.78 KB |
| PublishEvents | 100000  |  3,292.051 us | 1,173.5470 us |  64.3261 us |  31254.66 KB |
| PublishEvents | 1000000 | 35,038.627 us | 7,506.6267 us | 411.4635 us | 312505.48 KB |

### Pré-requis

Principalement du café et optionellement un clavier

### Installation

```sh {"id":"01J2KXAXNW7XF4PPWYZPVX70QG"}

 dotnet add package PEzBus


```

## Démarrage

L'utilisation est plutôt simple :

- Créer des classes représentants des évènements (accessoirement avec des attributs pour la transmission de donnée) qui implémente l'interface IEzEvent. L'idéale est de choisir des noms claires qui définissent l'évènement en question : i.e OrderCreatedEvent

- Implémenter des méthodes décorées avec l'attribut Subscribe comme suit :

```cs {"id":"01J2KXAXNW7XF4PPWYZQS3BWBZ"}
public class LaClass {
    [Subscribe(typeof(MonSuperEvent))]
    public void MaSuperMethode(){
        ...
    }
}

```

ou encore :

```cs {"id":"01J2KXAXNW7XF4PPWYZSSDTMHJ"}
public class LaClass {
   [Subscribe(typeof(MonSuperEvent))]
   public void MaSuperMethode(MonSuperEvent event){
       var eventArg = event.GetEventArg();
       eventArg.TransformationSayen();
   }
}

```

- Créer une instance du bus d'évènement :

```sh {"id":"01J2KXAXNW7XF4PPWYZTXSXBPX"}
EventBus eventBus = new();

```

- Inscrire une instance de la class dans ce bus d'évènement :

```ini {"id":"01J2KXAXNW7XF4PPWYZXPVRG3P"}
var maClass = new LaClass();
eventBus.Register<LaClass>(maClass);

```

- Publier l'évènement qui se produit :

```sh {"id":"01J2KXAXNW7XF4PPWYZXSY1Q4T"}
var event = new MonSuperEvent(...params);
eventBus.Publish(event);

```

Ensuite,les méthodes ayant comme paramètre de l'attribut Subscribe le type MonSuperEvent `[Subscribe(typeof(MonSuperEvent))]` seront invokés, si et seulement si une instance de classe contenant la méthode est inscrite dans le bus d'évènement.

## Versions

**Première version :** 1.0.1
Implémentation du bus d'évènements qui constitue le coeur du projet

## TODO :

Modéliser l'impacte que pourrait avoir un évènement sur les données existantes dans l'application en se basant sur le nom des évènements, le nom des méthodes, ainsi que les commentaires XML.

## Auteurs

* __Aymane EL JAHRANI__ _alias_ [@eljaymane](https://github.com/eljaymane)

## License


