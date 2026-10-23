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
- The guild and member records carry their real names and field meanings: the guilds a player
  belongs to, the roster, a unit's guild on its nameplate, and the recruiting guilds with their
  details and recruiters.
- Chat channel messages carry their real names and field meanings: joining and leaving, members,
  moderators, mutes, owners and passwords, the results of sending a line or a whisper, and the zone
  channels' zone.
- Chat lines carry their real names: a line sent, whispered or received, the server's acceptance of
  the player's own, and the links in a line (items, quests, archive articles, map points, loot, and
  the alien and profanity ranges).
- Unit messages carry their real names and field meanings: a unit's name, title, faction, stand
  state, path, guilds, target and vitals, emotes, vehicles and passengers, speech, loot taken,
  inspecting a unit, moving and swapping items, and the stun breakout.
- Spell messages carry their real names and field meanings: resurrection and the resurrect dialog,
  health, threat, CC states, spell thresholds, charges and cooldowns, floaters, the action bar's
  shortcut sets, stance, and sheathing weapons.
- Cast messages carry their real names and field meanings: the cast requests, a cast's start,
  confirmation, failure and finish, its targets and effects, spell visuals and missiles.
- Your character enters the world: each open-world zone is one shared map, a new character starts at
  its faction's start location, and where it stands, which way it faces and its zone are saved every
  minute while it moves and whenever it leaves. It stands alone for now; other players, creatures and
  chat come next.
- Log Out returns you to character select on the same connection, and Exit Game quits. Logging in
  again while your character is still in the world waits until it has been handed back, then enters
  where it stood.
- Operators give each faction a start location (`Realm:StartLocations`). The map settings under
  `Maps` (the tick, the update interval, the threads, the save interval and the queue drain) are
  checked at start, and a bad one stops the server, naming the setting.
- The stall watchdog watches every map as well as the account and realm owners.
- The protocol reference's opcode page lists every row with its routing tag, how far its message
  must be verified before the server reads it, and the largest body it accepts.
- Dash and the dash taken while knocked down, the unit stat updates, and the properties a new unit
  carries have their real names and field meanings.
- The path state's active path, XP list and unlocked mask name the path numbers they use: Soldier 0,
  Settler 1, Scientist 2 and Explorer 3.
- The combat log carries its real names and field meanings, kind by kind, and so do the player's
  spell list and the cast requests aimed at a position or at a unit.
- A new unit names its items, who tagged it, its minimap marker and, for a player, the character,
  realm, group, guild, title, PvP flags and premium tier. A unit's property and item look updates
  carry their real names, and the item look record is one shared by the character list and units.
- The protocol reference has a Host-settings page: every setting the server reads, its default, what
  the server checks at start and what it sets, generated from the settings themselves.

### Changed

- Work lands on `dev`, now the default branch, and `main` holds what has been seen working with the
  client. Every Tuesday, `main` takes the newest `dev` that has been proven.
- The World messages are grouped by the client's feature areas, in the code and in the protocol
  reference. Messages no area claims yet sit under Unsettled.
- A reference page the generator no longer writes keeps a tombstone that says so, instead of going
  stale.
- `Realm:Id` runs from 1 to 16383: 0 is refused at start, since the client refuses a character's
  unit that carries it.
- A message must travel under the routing tag its row names, and one under another tag ends the
  session. World messages the server doesn't handle yet are dropped and counted, and the session
  goes on.
- The server checks a message's row, its rate and the connection's state before reading its body,
  and a movement message larger than 2,048 bytes ends the session.
- Every setting's limits are checked in one place. A range failure at start reads like the others
  ("Gateway:Sts:Port must be 1 to 65535."), and a section reports all its failing settings at once.

### Fixed

- The combat log matches the client's layout: a reflected hit's sixth value is 32 bits, the dispel,
  transference and one amount kind end with their caster, target, spell and result, and kind 34 is
  declared.
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
- A chat line's alien and profanity links carry their whole 32-bit value, as the client reads them,
  in a line sent, whispered or received.
- A resurrection request reads as the client writes it: the type in 7 bits, then the value the type
  takes.
- A formatted floater's text reads as the client reads it, one text block in place of a list of
  them.

[Unreleased]: https://github.com/NexusTomorrow/server/compare/main...dev
