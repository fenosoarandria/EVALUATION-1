function updateTravaux(prix, designation, id_travaux) {
    fetch('/Payement/FrontOffice', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ prix, designation, id_travaux })
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            alert(data.message);
            // Traitez la réponse réussie (par exemple, mise à jour de l'UI)
        } else {
            alert(data.message);
            // Traitez l'erreur (par exemple, afficher un message d'erreur)
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Une erreur s\'est produite. Veuillez réessayer plus tard.');
    });
}
