import React, { Component } from 'react';
import GoogleMapReact from 'google-map-react';

const AnyReactComponent = ({ text }) => <div>{text}</div>;
// 9.009351537015766, 38.74461550509071
class Map extends Component {
  static defaultProps = {
    center: {
      lat: 9.009351537015766,
      lng: 38.74461550509071,
    },
    zoom: 11,
  };

  render() {
    return (
      <div style={{ height: '60vh', width: '100%' }}>
        <iframe
         src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3940.5827387735294!2d38.74247609999999!3d9.0104863!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x164b853ab014efd9%3A0x13a7cbc0872dd85f!2zRXRoaW9waWFuIENoYW1iZXIgb2YgQ29tbWVyY2UgYW5kIFNlY3RvcmFsIEFzc29jaWF0aW9ucyB8IOGLqOGKouGJteGLruGMteGLqyDhi6jhipXhjI3hi7XhipMg4Yuo4YuY4Yit4Y2NIOGIm-GKheGJoOGIq-GJtSDhiJ3hiq3hiK0g4Ymk4Ym1!5e0!3m2!1sen!2set!4v1685348664943!5m2!1sen!2set"
          width="600"
          height="450"
          allowFullScreen=""
          loading="lazy"
          referrerPolicy="no-referrer-when-downgrade"
          nonce="7331947154"
        ></iframe>
      </div>
    );
  }
}

export default Map;
