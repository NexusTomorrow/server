# Changelog

Every change to NexusTomorrow that a player or a server operator would notice, newest first. The
format follows [Keep a Changelog 1.1.0](https://keepachangelog.com/en/1.1.0/), and each release is
named for its date. **Unreleased** is what `dev` holds that `main` doesn't yet.

## [Unreleased]

### Added

- Debug builds carry a test endpoint for developers, off unless `Tools:Oracle:Enabled` is set and
  bound to loopback only behind a token written at each start. It sends any server message to a
  logged-in session, records the session's messages both ways, leaving out the bytes of any that
  carries a session key, a login token or a password, and lists a message's members. Release builds
  contain none of it, and the test workflow checks that.
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
- The protocol reference has a Codec-diagnostics page: every diagnostic the codec's source generator
  reports, how severe it is and when it fires, generated from the diagnostics themselves.
- In a Debug build, the developer tools' tap records each message the client sent with its decoded
  fields, read by the server or, for one the server dropped, by the tap, and the layout listing
  names each union arm's members and the member that counts each array.
- Path messages carry their real names and field meanings: path XP, changing and unlocking paths,
  episodes and mission progress, the explorer's power maps and progress reports, the scientist's
  scanbot, experimentation and creature data, the settler's builds, hubs and infrastructure, and the
  soldier's holdouts, builds and burrows, with the explorer's scavenger clues.
- Matchmaking messages carry their real names and field meanings: joining and leaving queues, role
  checks, the pending match, the match itself, votes to kick or surrender, penalties and PvP kills.
- Friends, rivals and ignores, account friends and their invites, presence and the friends list's
  status messages carry their real names and field meanings. The group's target markers message
  sits with the group messages.
- A unit's scan rewards, its settler improvement and its minimap marker updates carry their real
  names, and a type-23 unit names its group, guild and title as a player's does.
- Marketplace messages carry their real names and field meanings: commodity orders, item auctions,
  bids and searches, CREDD exchange orders and their history, and the marketplace's status. Order
  times are Windows file times.
- The message pages mark, in one Mark column, each message whose layout the client takes as
  declared, and each message the 16042 client never acts on, which the server neither sends nor
  handles. Directions read "to server", "to client" and "both ways".
- A chat link to a map point names its second coordinate Z.
- The tether unit message (0x0881) has its name and meaning, and seven messages the client receives
  and ignores are marked dormant.
- Cinematic messages carry their real names and field meanings: starting, continuing and skipping a
  cinematic, its cameras, actors, transitions and visual effects, and the white-out; clearing a map's
  destination marker sits with the map messages.
- Account inventory messages carry their real names and field meanings: account currencies,
  entitlements, account items and their cooldowns, pending gifts, and account operations.
- The movement commands a unit's movement is built from carry their real names where two readings
  agree (time, platform, position, velocity, commanded velocity, rotation, state and mode), and the
  player's movement message says which commands the client sends. A new unit names its creature,
  vendor flag and prerequisite, owner, vehicle, warplot tier and plug, and the time it was created.
- Instance messages carry their real names and field meanings: the instance settings window, resets
  and their results, the restricted window, the busy flag, leaving a pending-removal instance and the
  raid info request.
- Map messages carry their real names and field meanings: the zone map's explored hexes and hex
  groups, destination markers, taxi nodes, group members' positions, tracked units, phases and
  planes, the generic map and its nodes, and datacube progress. Hiding the story panel sits with the
  story messages.
- Costume messages carry their real names and field meanings: the costume list and updates, wearing a
  costume, saving one and its result, unlocking and forgetting costume items, and the appearance
  change.
- The unit movement message (0x0638) is marked as one whose layout the client takes as declared,
  every movement command included.
- Pet messages carry their real names and field meanings: pets summoned and dismissed, stances,
  customizations and their results, flair, vanity pets learned and summoned, and the pets in the
  player's state.
- A whisper names its target's realm: what the player types after the target's name and an at sign,
  a hyphen or a full stop.

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
- The Host-settings page says it lists the settings a Release build reads: a Debug build also reads
  the developer tools' settings, which the page leaves out.
- Comments restated in our own words.
- The README's steps run the server as a Release build.
- Friendship's last-online values are named in days, which lifts two messages to Fields; the account
  friend update returns to Purpose while its characters' race field waits on a second reading.
- The facts check's split test says it holds layout-confirmed messages as well as those at Fields,
  and its line counts both.

### Fixed

- Two account inventory messages that carried each other's meanings carry their own.

- The combat log matches the client's layout: a reflected hit's sixth value is 32 bits, the dispel,
  transference and one amount kind end with their caster, target, spell and result, and kind 34 is
  declared.
- Matchmaking messages match the client's layout: each entry of the queued-players list has a 2-bit
  sixth value, and a PvP kill's killer can be of a third kind.
- In a Debug build, the developer tools can set a message's float values.
- In a Debug build, the developer tools' tap leaves out the login token and chat channel passwords,
  the endpoint closes a connection that sends no token in time or stops reading, and a failed
  request is answered instead of dropping the connection.
- A game table column that follows a string reads its own value when the string sits after an odd
  number of 4-byte columns.
- Marketplace messages match the client's layout: a CREDD history entry has no trailing 32-bit
  value, and an auction search's rune slot filter takes a 32-bit minimum.
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
