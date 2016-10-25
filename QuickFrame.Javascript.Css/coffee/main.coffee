# CoffeeScript

search = (searchBox) ->
    if $(searchBox).val()
        window.location.href = $(searchBox).attr('search-url') + '?searchTerm=' + $(searchBox).val()
    else
        window.location.href = $(searchBox).attr('search-url')

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

    $('#searchBox').change ->
        search this

    $('#searchBox').keypress (event)->
        keycode = if event.keyCode then event.keyCode else event.which
        if(keycode == 13)
            search this
