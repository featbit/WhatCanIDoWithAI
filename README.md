Problems:

1. APIs generation sometimes lacks enough test data. We need to separate data generation logic from the API file to improve separation of concerns.
2. Generated code sometimes contains errors that need manual fixing.
3. API files were importing memoryDB from the wrong folder. Fixed the import path from "../../db/memoryDB" to "../db/memoryDB" as specified.
4. Always has unfinished generated file. It's because it's too long maybe.
5. NPM lib missing - **Need to restrict the libs that app use**
6. Menu items and page path mismatch