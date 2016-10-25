# CoffeeScript
loadDropdown = (selectControl, data)->
    if data
        objData = data.$values || data
    else
        return

    idField = selectControl.attr('data-id') || 'id'
    textField = selectControl.attr('data-text') || 'name'
    initialBlank = if selectControl.attr('data-placeholder') then true else false
    initialText = selectControl.attr('data-initial-text')
    initialVal = selectControl.attr('data-initial-val')
    currentOptions = [];
    selectControl.find('option').each ->
        currentOptions.push $(this).val()
    selectControl.empty()
        
    if initialBlank
        opt = $('<option>').text('')
        selectControl.append(opt)
            
    for d in objData
        id = d[idField]
        text = d[textField]
        opt = $('<option>', { value : id }).text(text)
        if (currentOptions && currentOptions.length > 0) && $.inArray(id.toString(), currentOptions) == -1
            opt.attr('selected', 'selected')
        if initialText && initialText == text
            opt.attr('selected', 'selected')
        if initialVal && initialVal.toString() == id.toString()
            opt.attr('selected', 'selected')
        selectControl.append(opt)
    selectControl.trigger 'chosen:updated'
    return

$(document).ready ->
    $('select[data-url]').each ()->
        _this = $(this)
        opts = {}

        if _this.attr('data-placeholder')
            opts.allow_single_deselect = true
        
        opts.width = _this.attr('data-width') || '100%'

        _this.chosen opts

        $('#overlay').removeClass('overlay-off').addClass('overlay')

        $.ajax
            method: 'GET',
            url: _this.attr('data-url')
        .done (html)->
            $('#overlay').removeClass('overlay').addClass('overlay-off')
            loadDropdown(_this, html)
    return;