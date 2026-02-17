import React, { useState } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Grid, Snackbar } from "@material-ui/core";
import { Button, Form, InputGroup } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";

import Footer from "../../component/Footer/Footer";
import { AddAlert } from "@material-ui/icons";
import { verifyCertificate } from "../../services/serviceRequest";
import { faCheckCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useParams } from "react-router-dom";

const useStyle = makeStyles((theme) => ({
  main: {
    width: "100%",
    background: "#fff",
    padding: "10px",
  },
}));

export default function VerifyCertificate() {
  const [code, setCode] = useState("");
  const [error, setError] = React.useState(false);
  const classes = useStyle();
  let params = useParams();
  React.useEffect(() => {
    if (params.code) {
      setCode(params.code.trim());
    }
  }, [params]);

  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
          }, 6000);
        }
        break;
      default:
        break;
    }
  };

  function handleSubmit(event) {
    const date = new Date();
    verifyCertificate(code)
      .then((data) => {
        const url = data.request.responseURL;// window.URL.createObjectURL(new Blob([data.data]));
        const link = document.createElement("a");
        link.href = url;
        /* link.setAttribute(
          "download",
          `certificate_${date.getFullYear()}${date.getMonth()}${date.getDay()}${date.getHours()}${date.getMinutes()}${date.getSeconds()}.pdf`
        ); */ //or any other extension        
        link.setAttribute(
          "target",
          `_blank`
        );
        document.body.appendChild(link);
        link.click();
        link.remove();
      })
      .catch((error) => {
        console.log(error.response);
        if (error.response) {
          // setErrorMsg(error.response.data.toString());
          showNotification("tr");
        }
      });

    event.preventDefault();
  }
  return (
    <div class="mmenu">
      <div className={classes.main}>
        <Grid container spacing={2} className="grid">
          <Grid item xs={3} md={3}></Grid>
          <Grid item xs={6} md={6}>
            <div class="row">
              <div class="bg" style={{ fontSize: "20px", marginTop: "0" }}>
                Verify Certificate{" "}
              </div>
            </div>
            <form onSubmit={handleSubmit}>
              <Form.Group size="lg" controlId="userName">
                <InputGroup className="mb-3">
                  <InputGroup.Text>Signature</InputGroup.Text>
                  <Form.Control
                    autoFocus
                    type="text"
                    value={code}
                    onChange={(e) => setCode(e.target.value)}
                  />
                  <Button
                    type="submit"
                    variant="outline-success"
                    style={{ margin: "0" }}
                    id="button-addon2"
                  >
                    <FontAwesomeIcon icon={faCheckCircle} />
                  </Button>
                </InputGroup>
              </Form.Group>
            </form>
          </Grid>
          <Grid item xs={3} md={3} className={classes.counter}></Grid>

          <br />
          <br />
          
          <Snackbar
            place="tr"
            color="danger"
            icon={AddAlert}
            message="Failed to verify"
            open={error}
            closeNotification={() => setError(false)}
            close
          />
        </Grid>
      </div>
    </div>
  );
}
