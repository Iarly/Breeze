[![Build Status](https://secure.travis-ci.org/kriskowal/q.png)](http://travis-ci.org/kriskowal/q)

If a function cannot return a value or throw an exception without
blocking, it can return a promise instead.  A promise is an object
that represents the return value or the thrown exception that the
function may eventually provide.  A promise can also be used as a
proxy for a [remote object][Q-Comm] to overcome latency.

[Q-Comm]: https://github.com/kriskowal/q-comm

On the first pass, promises can mitigate the “[Pyramid of
Doom][POD]”: the situation where code marches to the right faster
than it marches forward.

[POD]: http://calculist.org/blog/2011/12/14/why-coroutines-wont-work-on-the-web/

```javascript
step1(function (value1) {
    step2(value1, function(value2) {
        step3(value2, function(value3) {
            step4(value3, function(value4) {
                // Do something with value4
            });
        });
    });
});
```

With a promise library, you can flatten the pyramid.

```javascript
Q.call(step1)
.then(step2)
.then(step3)
.then(step4)
.then(function (value4) {
    // Do something with value4
}, function (error) {
    // Handle any error from step1 through step4
})
.end();
```

With this approach, you also get implicit error propagation,
just like ``try``, ``catch``, and ``finally``.  An error in
``step1`` will flow all the way to ``step5``, where it’s
caught and handled.

The callback approach is called an “inversion of control”.
A function that accepts a callback instead of a return value
is saying, “Don’t call me, I’ll call you.”.  Promises
[un-invert][IOC] the inversion, cleanly separating the input
arguments from control flow arguments.  This simplifies the
use and creation of API’s, particularly variadic,
rest and spread arguments.

[IOC]: http://www.slideshare.net/domenicdenicola/callbacks-promises-and-coroutines-oh-my-the-evolution-of-asynchronicity-in-javascript


Getting Started
===============

The Q module can be loaded as:

-   a ``<script>`` tag (creating a ``Q`` global variable): only ~2 KB
    minified and gzipped!
-   a NodeJS and CommonJS module available from NPM as the ``q``
    package
-   a RequireJS module

Q can exchange promises with jQuery and Dojo and the following libraries
are based on Q.

-   [q-fs](https://github.com/kriskowal/q-fs)
    file system
-   [q-http](https://github.com/kriskowal/q-http)
    http client and server
-   [q-comm](https://github.com/kriskowal/q-comm)
    remote objects
-   [jaque](https://github.com/kriskowal/jaque)
    promising HTTP server, JSGI middleware

[Many other projects](http://search.npmjs.org/#/q) in NPM use Q
internally or provide Q promises.

Please join the Q-Continuum [mailing list](https://groups.google.com/forum/#!forum/q-continuum).


Tutorial
========

Promises have a ``then`` method, which you can use to get the eventual
return value (fulfillment) or thrown exception (rejection).

```javascript
foo()
.then(function (value) {
}, function (reason) {
})
```

If ``foo`` returns a promise that gets fulfilled later with a return
value, the first function (the value handler) will be called with the
value.  However, if the ``foo`` function gets rejected later by a
thrown exception, the second function (the error handler) will be
called with the error.


## Propagation

The ``then`` method returns a promise, which in this example, I’m
assigning to ``bar``.

```javascript
var bar = foo()
.then(function (value) {
}, function (reason) {
})
```

The ``bar`` variable becomes a new promise for the return value of
either handler.  Since a function can only either return a value or
throw an exception, only one handler will ever be called and it will
be responsible for resolving ``bar``.

-   If you return a value in a handler, ``bar`` will get fulfilled.

-   If you throw an exception in a handler ``bar`` will get rejected.

-   If you return a **promise** in a handler, ``bar`` will “become”
    that promise.  Being able to become a new promise is useful for
    managing delays, combining results, or recovering from errors.

If the ``foo()`` promise gets rejected and you omit the error handler,
the **error** will go to ``bar``:

```javascript
var bar = foo()
.then(function (value) {
})
```

If the ``foo()`` promise gets fulfilled and you omit the value
handler, the **value** will go to ``bar``:

```javascript
var bar = foo()
.then(null, function (error) {
})
```

Q promises provide a ``fail`` shorthand for ``then`` when you are only
interested in handling the error:

```javascript
var bar = foo()
.fail(function (error) {
})
```

They also have a ``fin`` function that is like a ``finally`` clause.
The final handler gets called, with no arguments, when the promise
returned by ``foo()`` either returns a value or throws an error.  The
value returned or error thrown by ``foo()`` passes directly to ``bar``.

```javascript
var bar = foo()
.fin(function () {
    // close files, database connections, stop servers, conclude tests
})
```

-   If the handler returns a value, the value is ignored
-   If the handler throws an error, the error passes to ``bar``
-   If the handler returns a promise, ``bar`` gets postponed.  The
    eventual value or error has the same effect as an immediate return
    value or thrown error: a value would be ignored, an error would be
    forwarded.

## Chaining

There are two ways to chain promises.  You can chain promises either
inside or outside handlers.  The next two examples are equivalent.

```javascript
return foo()
.then(function (fooValue) {
    return bar(fooValue)
    .then(function (barValue) {
        // if we get here without an error,
        // the value returned here
        // or the exception thrown here
        // resolves the promise returned
        // by the first line
    })
})
```

```javascript
return foo()
.then(function (fooValue) {
    return bar(fooValue);
})
.then(function (barValue) {
    // if we get here without an error,
    // the value returned here
    // or the exception thrown here
    // resolves the promise returned
    // by the first line
})
```

The only difference is nesting.  It’s useful to nest handlers if you
need to capture both ``fooValue`` and ``barValue`` in the last
handler.

```javascript
function eventualAdd(a, b) {
    return a.then(function (a) {
        return b.then(function (b) {
            return a + b;
        });
    });
}
```


## Combination

You can turn an array of promises into a promise for the whole,
fulfilled array using ``all``.

```javascript
return Q.all([
    eventualAdd(2, 2),
    eventualAdd(10, 20)
])
```

If you have a promise for an array, you can use ``spread`` as a
replacement for ``then``.  The ``spread`` function “spreads” the
values over the arguments of the value handler.  The error handler
will get called at the first sign of failure.  That is, whichever of
the recived promises fails first gets handled by the error handler.

```javascript
function eventualAdd(a, b) {
    return Q.all([a, b])
    .spread(function (a, b) {
        return a + b;
    })
}
```

But ``spread`` calls ``all`` initially, so you can skip it in chains.

```javascript
return foo()
.then(function (name, location) {
    return [name, FS.read(location, "utf-8")];
})
.spread(function (name, text) {
})
```


## Handling Errors

One sometimes-unintuive aspect of promises is that if you throw an
exception in the value handler, it will not be be caught by the error
handler.

```javascript
foo()
.then(function (value) {
    throw new Error("Can't bar.");
}, function (error) {
    // We only get here if "foo" fails
})
```

To see why this is, consider the parallel between promises and
``try``/``catch``. We are ``try``-ing to execute ``foo()``: the error
handler represents a ``catch`` for ``foo()``, while the value handler
represents code that happens *after* the ``try``/``catch`` block.
That code then needs its own ``try``/``catch`` block.

In terms of promises, this means chaining your error handler:

```javascript
foo()
.then(function (value) {
    throw new Error("Can't bar.");
})
.fail(function (error) {
    // We get here with either foo's error or bar's error
})
```


## The End

When you get to the end of a chain of promises, you should either
return the last promise or end the chain.  Since handlers catch
errors, it’s an unfortunate pattern that the exceptions can go
unobserved.

So, either return it,

```javascript
return foo()
.then(function () {
    return "bar";
})
```

Or, end it.

```javascript
foo()
.then(function () {
    return "bar";
})
.end()
```

Ending a promise chain makes sure that, if an error doesn’t get
handled before the end, it will get rethrown and reported.

This is a stopgap. We are exploring ways to make unhandled errors
visible without any explicit handling.


## The Beginning

Everything above assumes you get a promise from somewhere else.  This
is the common case.  Every once in a while, you will need to create a
promise from scratch.

You can create a promise from a value using ``Q.call``.  This returns a
promise for 10.

```javascript
return Q.call(function () {
    return 10;
});
```

You can also use ``call`` to get a promise for an exception.

```javascript
return Q.call(function () {
    throw new Error("Can't do it");
})
```

As the name implies, ``call`` can call functions, or even promised
functions.  This uses the ``eventualAdd`` function above to add two
numbers.  The second argument is the ``this`` object to pass into the
function.

```javascript
return Q.call(eventualAdd, null, 2, 2);
```

When nothing else will do the job, you can use ``defer``, which is
where all promises ultimately come from.

```javascript
var deferred = Q.defer();
FS.readFile("foo.txt", "utf-8", function (error, text) {
    if (error) {
        deferred.reject(new Error(error));
    } else {
        deferred.resolve(text);
    }
});
return deferred.promise;
```

Note that a deferred can be resolved with a value or a promise.  The
``reject`` function is a shorthand for resolving with a rejected
promise.

```javascript
var rejection = Q.call(function () {
    throw new Error("Can't do it");
});
deferred.resolve(rejection);
```

This is a simplified implementation of ``Q.delay``.

```javascript
function delay(ms) {
    var deferred = Q.defer();
    setTimeout(deferred.resolve, ms);
    return deferred.promise;
}
```

This is a simplified implementation of ``Q.timeout``

```javascript
function timeout(promise, ms) {
    var deferred = Q.defer();
    Q.when(promise, deferred.resolve);
    Q.when(delay(ms), function () {
        deferred.reject("Timed out");
    });
    return deferred.promise;
}
```


## The Middle

If you are using a function that may return a promise, but just might
return a value if it doesn’t need to defer, you can use the “static”
methods of the Q library.

The ``when`` function is the static equivalent for ``then``.

```javascript
return Q.when(valueOrPromise, function (value) {
}, function (error) {
});
```

All of the other methods on a promise have static analogs with the
same name.

The following are equivalent:

```javascript
return Q.all([a, b]);
```

```javascript
return Q.call(function () {
    return [a, b];
})
.all();
```

When working with promises provided by other libraries, you should
convert it to a Q promise.  Not all promise libraries make the same
guarantees as Q and certainly don’t provide all of the same methods.
Most libraries only provide a partially functional ``then`` method.
This thankfully is all we need to turn them into vibrant Q promises.

```javascript
return Q.when($.ajax(...))
.then(function () {
})
```

If there is any chance that the promise you receive is not a Q promise
as provided by your library, you should wrap it using a Q function.
You can even use ``Q.call`` as a shorthand.

```javascript
return Q.call($.ajax, $, ...)
.then(function () {
})
```


## Over the Wire

A promise can serve as a proxy for another object, even a remote
object.  There are methods that allow you to optimistically manipulate
properties or call functions.  All of these interactions return
promises, so they can be chained.

```
direct manipulation         using a promise as a proxy
--------------------------  -------------------------------
value.foo                   promise.get("foo")
value.foo = value           promise.put("foo", value)
delete value.foo            promise.del("foo")
value.foo(...args)          promise.post("foo", [args])
value.foo(...args)          promise.invoke("foo", ...args)
value(...args)              promise.apply(null, [args])
value(...args)              promise.call(null, ...args)
value.call(thisp, ...args)  promise.apply(thisp, [args])
value.apply(thisp, [args])  promise.call(thisp, ...args)
```

If the promise is a proxy for a remote object, you can shave
round-trips by using these functions instead of ``then``.  To take
advantage of promises for remote objects, check out [Q-Comm][].

[Q-Comm]: https://github.com/kriskowal/q-comm

Even in the case of non-remote objects, these methods can be used as
shorthand for particularly-simple value handlers. For example, you
can replace

```javascript
return Q.call(function () {
    return [{ foo: "bar" }, { foo: "baz" }];
})
.then(function (value) {
    return value[0].foo;
})
```

with

```javascript
return Q.call(function () {
    return [{ foo: "bar" }, { foo: "baz" }];
})
.get(0)
.get("foo")
```


## Adapting Node

There is a ``node`` method on deferreds that is handy for the NodeJS
callback pattern.

```javascript
var deferred = Q.defer();
FS.readFile("foo.txt", "utf-8", deferred.node());
return deferred.promise;
```

And there’s a ``Q.ncall`` function for shorter.

```javascript
return Q.ncall(FS.readFile, FS, "foo.txt", "utf-8");
```

There is also a ``Q.node`` function that that creates a reusable
wrapper.

```javascript
var readFile = Q.node(FS.readFile, FS)
return readFile("foo.txt", "utf-8");
```


Reference
=========

A method-by-method [Q API reference][reference] is available on the wiki.

[reference]: q/wiki/API-Reference

---

Copyright 2009-2011 Kristopher Michael Kowal
MIT License (enclosed)
