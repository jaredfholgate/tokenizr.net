# tokenizr.net

[![Build status](https://dev.azure.com/jaredfholgate/tokenizr.net/_apis/build/status/tokenizr.net)](https://dev.azure.com/jaredfholgate/tokenizr.net/_build/latest?definitionId=-1) 

C# project for tokenization of strings for security in apps.

## What is Tokenisation?

Tokenisation is a layer of security abstraction applied at the application logic level. It allows individual fields to be persisted to the database as 'scrambled' text, which can only be reversed to their true value by leveraging the tokenisation service. Tokenisation can be used where tradistional cryptography may be hard, for example if you are unable to change the database schema of your application, you can still store data in a tokenised form and it will meet the validation rules in your databases. E.g. String length, characters allowed, etc.

Tokenisation also allows a limited level of search indexing of these fields too. Generally, you would only be able to search for the whole word / sentence, or in a less secure mode you can also search the start for partial phrases or words.

Tokenisation is not a good candidate for security in most cases. It is generally used in the card payment or similar industry where there is no requirment to search text and people really don't want to store card details in their own database.

### Limitations

Tokenisation is reliant on effective separation of the tokenisation service from the application using it. The Tokenisation service must be completely secure. If the service is compromised, then so is your tokenised data. Tokenisation is not a replacement for other secure methods of securing data, but can be used as an additional layer on top for very sensitive data. 

This particular implementation of tokenisation is not designed to be robust security, it is a layer of obfuscation on top of other technologies, such as TDE to enusre that someone who gets hold of an unencrypted copy of the production database cannot easily get hold of the real production data for the protected fields.

## Usage
The package consists of two main components. A generator and the tokenization client.

### Generator
The Generator is used to generate a static randomised table set. This table set is used to tokenise and detokenise without the need to persist key value pairs in a database. This means that a client API would not need to worry about persisting tokenised values, rather it would just store the the table set in a static encrypted file. 

There are various option when generating tables, such as whether to include punctuation, spaces and special characters. Depending on the field or set of fields you want to tokenise, you need to think about this in advance.

### Client

The client consists of 2 main methods Tokenise and Detokenise. You can probably guess what they do.

There is an option for consistency. This is for an edge case where you might want to search on the start of a tokenised field. It will consistently tokenise from the start of the text, but is obviously less secure.

There is an option to supply a mask, so that only parts of a field are tokenised, for example you may want to retain the last 4 digits of a card number to confirm with the user that is the card they want to use.

### Generator Examples

//SOON COME!

### Client Examples

//SOON COME!