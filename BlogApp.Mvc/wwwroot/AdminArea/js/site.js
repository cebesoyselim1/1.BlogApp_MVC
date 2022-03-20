function ConvertFirstLetterToUpperCase(text){
    return text[0].toUpperCase() + text.slice(1);
}

function ConvertToShortDate(dateString){
    return new Date(dateString).toLocaleDateString("en-US");
}