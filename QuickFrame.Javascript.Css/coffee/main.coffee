# CoffeeScript

#jQuery.extend
#    closeFancybox: ()->
#        parent.$.fancybox.close()

jQuery.extend
    closeAndRefreshParent: ()->
        parent.$.fancybox.close()
        parent.$.reloadWindow()

#jQuery.extend
#    reloadWindow: ()->
#        window.location.reload();

$(document).ready ->
    $('.datepicker').each ->
        $(this).datepicker()

    $('.chosen').each ->
        ($this).chosen()

    $('.remove-object').each ->
        $(this).on 'click', (e)->
            e.preventDefault();
            if(confirm('Are you sure you wish to delete this object?'))
                $.ajax
                    url: $(this).attr('href'),
                    method: 'DELETE',
                    success: ->
                        window.location.reload()

    $('[email-src]').each ->
        $(this).blur ->
            if $(this).val().length > 0
                dest = '#' + $(this).attr('email-dest')
                $(dest).val($(this).val().replace(' ', '.') + '@ffspaducah.com')

    $('.fancybox').each ->
        _this = $(this)
        refresh = $(this).is('[refresh]')
        opts = {}
        opts.type = 'iframe'
        opts.autoSize = false
        opts.width = $(this).attr('data-width') || '640px'
        opts.height = $(this).attr('data-height') || '480px'
        opts.closeBtn = $(this).is('[close-button]')
        opts.iframe = {}
        if $(this).is('[scrolling]')
            opts.iframe.scrolling = 'yes'
        else
            opts.iframe.scrolling = 'no'
        opts.afterClose = ->
            if refresh
                window.location.reload()

        $(this).fancybox opts

