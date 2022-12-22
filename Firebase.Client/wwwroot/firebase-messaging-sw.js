/*
 * Give the service worker access to Firebase Messaging.
 * Note that you can only use Firebase Messaging here, other Firebase libraries are not available in the service worker.
*/


// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/9.8.2/firebase-app.js";
import { getMessaging, getToken, onMessage } from "https://www.gstatic.com/firebasejs/9.8.2/firebase-messaging.js";


// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
    apiKey: "AIzaSyB7f9amBHcn916Gsj6Lfrelm3Kgv9Has0s",
    authDomain: "wewe-7711d.firebaseapp.com",
    projectId: "wewe-7711d",
    storageBucket: "wewe-7711d.appspot.com",
    messagingSenderId: "339343575839",
    appId: "1:339343575839:web:20ae62f35146a74145be2a",
    measurementId: "G-TC7X4PK0P6"
};
// Initialize Firebase
const app = initializeApp(firebaseConfig);

/*
 * Retrieve an instance of Firebase Messaging so that it can handle background messages.
*/
const messaging = getMessaging(app);
messaging.setBackgroundMessageHandler(function (payload) {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);

    // Customize notification here
    const notificationTitle = payload.data.title;
    const notificationOptions = {
        body: payload.data.body,
        icon: '',
        image: ''
    };

    return self.registration.showNotification(
        notificationTitle,
        notificationOptions
    );
});
