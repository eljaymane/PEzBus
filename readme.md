# PEzBus

[![forthebadge](http://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)  [![forthebadge](http://forthebadge.com/images/badges/powered-by-electricity.svg)](http://forthebadge.com)

Certaines applications traitant de grands flux d'entrées/sorties de données (i.e les OMS, serveurs de jeux videos ...) nécessitent une grande organisation pour orchestrer l'impacte d'une nouvelle donnée ou action sur les données existantes. Dans ce cas précis, développer un système drivé par les évènements permet de créer un effets dominos et mettre un nom sur ces changements.

Le java a son package Guava de google, qui offre un bus d'évènement qui implémente le pattern Publish / Subscribe et le rend très accessible grâce aux décorateurs de méthodes :

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
    [Subscribe(typeof(MonSuperEvent))]
    public void MaSuperMethode(){
        ...
    }
    ```
    ou encore : 

     ```
    [Subscribe(typeof(MonSuperEvent))]
    public void MaSuperMethode(MonSuperEvent event){
        var eventArg = event.GetEventArg();
        eventArg.TransformationSayen();
    }
    ```
- Créer une instance du bus d'évènement : 
```
EventBus eventBus = new();
```
- Publier l'évènement qui se produit : 
```
var event = new MonSuperEvent(...params);
eventBus.Publish(event);
```

Ensuite, toutes les méthodes ayant paramètre de l'attribut Subscribe le type MonSuperEvent ```[Subscribe(typeof(MonSuperEvent))]``` seront invokés.




## Versions

**Première version :** 1.0.0
Implémentation du bus d'évènements qui constitue le coeur du projet

## TODO : 
Modéliser l'impacte que pourrait avoir un évènement sur les données existantes dans l'application en se basant sur le nom des évènements, le nom des méthodes, ainsi que les commentaires XML.

## Auteurs

* **Aymane EL JAHRANI** _alias_ [@eljaymane](https://github.com/eljaymane)


## License


