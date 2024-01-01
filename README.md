# ChatPlugin

A plugin based on Exiled for SCP:SL. This plugin allows player use keyboard to communicate with teammates.
This plugin send message to same side as player's.

# There's currently no English support. If you need it, Always welcom to pull a request about it or open a issue for that. I'll do if i have time.

## Legacy Chat Mode

Usage:

```
.say [Content] Channel
```

Example:

```
.say [Hello World] 3
```

Channels:

1 Same Side

2 All Human

3 All Players

## New Chat Mode

### New Chat Mode does not support space between contents. It will seperate your words. Use space carefully.

### Faction Chat

Usage:

```
.say Content
```

Example:

```
.say HelloWorld
```


## Global Chat

Usage:

```
.gsay Content
```

Example:

```
.gsay HelloWorld
```

# Configuration
```
chat_plugin:
# Whether or not this plugin is enabled.
is_enabled: true
# Whether or not to display debug messages in the server console.
debug: false
# Use old chat mode instead of new one.
legacy_chat_mode: false
# Set true to Broadcast to use Broadcast. Otherwise will be Hint.
human_chat: false
# Set true to Broadcast to use Broadcast. Otherwise will be Hint.
scp_chat: true
```







