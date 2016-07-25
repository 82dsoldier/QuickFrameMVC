# CoffeeScript
#reloadParent = ->
#    parent.location.reload()
#    return

#closePopup = ->
#    parent.$.fancybox.close()
#    return
showOverlay = ->
    $('#overlay').addClass('overlay')

hideOverlay = ->
    $('#overlay').removeClass('overlay').addClass('overlay-off');

$(document).ready ->
    $('input[type=text]').first().focus()

