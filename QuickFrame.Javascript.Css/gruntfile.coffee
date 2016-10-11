# <binding AfterBuild='build' />
# CoffeeScript
module.exports = (grunt)->
    require('load-grunt-tasks') grunt 

    grunt.initConfig
        properties: grunt.file.readJSON('properties.json'),
        concat:
            js:
                src: [
                    'jquery/jquery-2.2.3.js',
                    'jquery-ui/1.11.4/jquery-ui.js'
                    'bootstrap/3.3.6/bootstrap.js',
                    'jquery-validation/1.15.0/jquery.validate.js',
                    'jquery-validation/1.15.0/additional-methods.js',
                    'fancybox/2.1.5/jquery.fancybox.js',
                    'chosen/1.5.1/chosen.jquery.js',
                    'wwwroot/js/dropdown.js',
                    'wwwroot/js/main.js'
                    ],
                dest: 'wwwroot/js/jquery-bundle.js'
            css:
                src: [
                    'jquery-ui/1.11.4/jquery-ui.css',
                    'jquery-ui/1.11.4/jquery-ui.structure.css',
                    'jquery-ui/1.11.4/jquery-ui.theme.css',
                    'bootstrap/3.3.6/bootstrap.css',
                    'bootstrap/3.3.6/bootstrap-theme.css',
                    'font-awesome/4.6.3/font-awesome.css',
                    'fancybox/2.1.5/jquery.fancybox.css',
                    'chosen/1.5.1/chosen.css'
                    'wwwroot/css/site.css'
                    ],
                dest: 'wwwroot/css/site-bundle.css'
        sass:
            all:
                files:
                    'wwwroot/css/site.css': ['Sass/Site.scss']
        copy:
            jquery:
                expand: true,
                flatten: true,
                cwd: 'jquery-ui/1.11.4/images/',
                src: ['**'],
                dest: 'wwwroot/images/',
                filter: 'isFile'
            bootstrap:
                expand: true,
                flatten: true,
                cwd: 'bootstrap/3.3.6/fonts',
                src: ['**'],
                dest: 'wwwroot/fonts',
                filter: 'isFile'
            fontawesome:
                expand: true,
                flatten: true,
                cwd: 'font-awesome/4.6.3/fonts',
                src: ['**'],
                dest: 'wwwroot/fonts',
                filter: 'isFile'
            fancybox:
                expand: true,
                flatten: true,
                cwd: 'fancybox/2.1.5/images',
                src: ['**'],
                dest: 'wwwroot/images',
                filter: 'isFile'
            chosen:
                expand: true,
                flatten: true,
                cwd: 'chosen/1.5.1/images',
                src: ['**'],
                dest: 'wwwroot/images',
                filter: 'isFile'
             js:
                expand: true,
                flatten: true,
                cwd: 'wwwroot/js',
                src: ['**'],
                dest: '<%= properties.dest_dir %>/js',
                filter: 'isFile'
            css:
                expand: true,
                flatten: true,
                cwd: 'wwwroot/css',
                src: ['**'],
                dest: '<%= properties.dest_dir %>/css',
                filter: 'isFile'
            images:
                expand: true,
                flatten: true,
                cwd: 'wwwroot/images',
                src: ['**'],
                dest: '<%= properties.dest_dir %>/images',
                filter: 'isFile'
            fonts:
                expand: true,
                flatten: true,
                cwd: 'wwwroot/fonts',
                src: ['**'],
                dest: '<%= properties.dest_dir %>/fonts',
                filter: 'isFile'
        cssmin:
            options:
                keepSpecialComments: 0
            all:
                expand: true
                flatten: true
                cwd: 'wwwroot/css',
                src: ['*.css', '!*.min.css'],
                dest: 'wwwroot/css',
                ext: '.min.css'
        uglify:
            options:
                mangle:
                    except: ['jQuery', 'AbstractChosen', 'Chosen', 'SelectParser', 'angular']
            all:
                expand: true,
                flatten: true,
                cwd: 'wwwroot/js',
                src: ['*.js', '!*.min.js'],
                dest: 'wwwroot/js',
                ext: '.min.js'
        coffee:
            all:
                expand: true,
                flatten: true,
                cwd: 'coffee',
                src: ['*.coffee'],
                dest: 'wwwroot/js/',
                ext: '.js'
    grunt.registerTask 'copy-first', [ 'copy:jquery', 'copy:bootstrap', 'copy:fontawesome', 'copy:fancybox', 'copy:chosen']
    grunt.registerTask 'copy-final', [ 'copy:js', 'copy:css', 'copy:images', 'copy:fonts' ]
    grunt.registerTask 'build', ['coffee', 'copy-first', 'concat', 'cssmin', 'uglify', 'copy-final' ]
    grunt.registerTask 'default', [ 'build' ]
    return