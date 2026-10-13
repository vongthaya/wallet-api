# Wallet API
A REST API for a simulated Lao Kip (LAK) wallet. It manages users, wallets, and balances. The project does not connect to real banks. This is a learning/portfolio project, work in progress.

## Tech stack
+ C#
+ .NET 10
+ ASP.NET Core
+ PostgreSQL 18
+ Entity Framework Core
+ xUnit
+ Docker

## Design decisions
+ **One wallet per user.** A `UNIQUE` index on `wallets.user_id` stops a user from having two wallets. Without it, the debit/credit would be applied twice.
+ **Don't delete a user having a wallet.** An `ON DELETE RESTRICT` on `wallets.user_id` stops deleting a user. Without it, the wallet would be silently removed.
+ **Balance must not be negative.** A `CHECK` constraint rejects negative balances. Without it, a user can transfer more than their balance.

## Roadmap
+ Run both updates of a transfer in a transaction. If the debit fails, the transaction will roll back.
