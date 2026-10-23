# Changelog

Every change to NexusTomorrow that a player or a server operator would notice, newest first. The
format follows [Keep a Changelog 1.1.0](https://keepachangelog.com/en/1.1.0/), and each release is
named for its date. **Unreleased** is what `dev` holds that `main` doesn't yet.

## [Unreleased]

### Added

- The protocol reference has a page for the gateway's opcode table and one for the admin commands,
  both generated from the code.
- More of the client's messages carry their real names and field meanings: mail, quests, the
  inventory, groups, housing, guilds and logout, plus the realm-scoped id that characters and
  residences go by and chat's channel key.
- Group invitations, join requests, referrals, mentoring, target markers, instance difficulty and
  raid lockouts carry their real names and field meanings.
- Deleting an item, salvaging one and taking items from the supply satchel carry their real names and
  field meanings.
- Housing requests carry their real names: residence options, neighbor invitations, evictions and
  permissions, visits, vendor lists, renaming a residence or a community, and community placement and
  privacy.
- The housing messages the client receives carry their real names: the residence list and privacy,
  neighbors and neighbor invitations, the random visit lists, a community's placed residences, vendor
  lists, ownership, plot builds and operation results.
- Guild requests carry their real names and field meanings: guild operations, creating a guild,
  holomarks and standards, the guild bank's tabs, money and items, perks, boss tokens and recruitment
  details. The vendor repair, which a guild can pay for, sits with the inventory messages.
- The guild state the client receives carries real names and field meanings: the bank's tabs, items
  and today's withdrawals, ranks, the event log, perks, the PvP record and queue state, the standard,
  recruitment, and a war party's boss tokens and match results.

### Changed

- Work lands on `dev`, now the default branch, and `main` holds what has been seen working with the
  client. Every Tuesday, `main` takes the newest `dev` that has been proven.
- The World messages are grouped by the client's feature areas, in the code and in the protocol
  reference. Messages no area claims yet sit under Unsettled.
- A reference page the generator no longer writes keeps a tombstone that says so, instead of going
  stale.

### Fixed

- After logging in, the client no longer keeps reconnecting: the server answers its verified-address
  request.
- Game data read correctly where a 64-bit number follows an odd count of 32-bit ones: tradeskill
  relearn costs and character customization costs no longer come out four billion times too large.
- ClientConnector finds the client and `config.json` beside itself, whatever folder it is started
  from.
- A test run filtered to some tests no longer fails on the test projects the filter leaves empty,
  and one whose filter matches nothing now fails.
- A war plot match's results carry both sides' values, and a guild's event log entries carry no guild
  id, as the client reads them.
- Items added to a guild bank, and a bank tab's items, carry the item alone, without the extra value
  only an item added to the player's own inventory carries.

[Unreleased]: https://github.com/NexusTomorrow/server/compare/main...dev
