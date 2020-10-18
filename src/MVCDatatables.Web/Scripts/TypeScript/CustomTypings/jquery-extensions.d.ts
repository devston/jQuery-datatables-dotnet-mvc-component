/**
 * This file contains all the custom type extensions for jQuery.
 */
interface JQuery {
    timeout(ms: number, callback?: Function): JQuery;
}