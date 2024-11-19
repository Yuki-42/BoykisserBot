# Planning Doc

This document has no real order, it's just a place for me to brainstorm ideas without having to leave the IDE.

## Expeditions (Quests)

Expeditions are a system in which you can send out 3 of your owned characters and they will return with a new character
after a certain amount of time. The UI will use discord message-components to allow the user to select up to 3
characters
to send on the expedition. The (internal) expedition level is a rounded average of the rarity of the characters sent on
the
expedition. The higher the expedition level, the longer the expedition will take (up to a maximum of 1 week). The user
will
be able to cancel the expedition at any time, but will not receive a new character if they do so.

## Character Drops

Characters will be able to drop from one of two methods: Expeditions and Time-Locked Drops

### Time-Locked Drops

Time-Locked Drops are the far simpler of the two methods. Every (interval) amount of time, the user can claim a new
character
from a weighted list of characters that they do not own. The weighting of the characters will be based on the rarity of
the
character. The user will be able to claim the character at any time, but will not be able to claim another character
until the
next interval.

I'm looking at several different methods for this, one of the best looking seems to be the Walker-Vose "Alias Method"
for

