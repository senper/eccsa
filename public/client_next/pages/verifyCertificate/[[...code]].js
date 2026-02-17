import React, { useState } from "react";
import { Button, Form, InputGroup } from "react-bootstrap";
import { verifyCertificate } from "../../api/serviceRequest";
import { faCheckCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { getLocalByKey } from "../api/menu";
import { Grid, Snackbar } from "@mui/material";
import { useRouter } from "next/router";
import { AddAlert } from "@mui/icons-material";

export default function VerifyCertificate({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  const [code, setCode] = useState("");
  const [error, setError] = React.useState(false);
  let codeValue = 0;
  if (location.query) {
    const { code } = location.query;
    codeValue = code;
  }
  React.useEffect(() => {
    if (codeValue && Array.isArray(codeValue) && codeValue[0]) {
      setCode(codeValue[0]);
    } else if (codeValue && codeValue.trim) {
      setCode(codeValue.trim());
    }
  }, [codeValue]);

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
        const url = data.request.responseURL; // window.URL.createObjectURL(new Blob([data.data]));
        const link = document.createElement("a");
        link.href = url;
        /* link.setAttribute(
          "download",
          `certificate_${date.getFullYear()}${date.getMonth()}${date.getDay()}${date.getHours()}${date.getMinutes()}${date.getSeconds()}.pdf`
        ); */ //or any other extension
        link.setAttribute("target", `_blank`);
        document.body.appendChild(link);
        link.click();
        link.remove();
      })
      .catch((error) => {
        // console.log(error.response);
        if (error.response) {
          // setErrorMsg(error.response.data.toString());
          showNotification("tr");
        }
      });

    event.preventDefault();
  }
  return (
    <>
      <Grid container style={{paddingTop: "15px"}}>
        <Grid item xs={3} md={3}></Grid>
        <Grid item xs={6} md={6}>
          <div className="row">
            <div className="bg" style={{ fontSize: "20px", marginTop: "0" }}>
              {getLocal("Verify Certificate")}{" "}
            </div>
          </div>
          <form onSubmit={handleSubmit}>
            <Form.Group size="lg" controlId="userName">
              <InputGroup className="mb-3">
                <InputGroup.Text>{getLocal("Signature")}</InputGroup.Text>
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
        <Grid item xs={3} md={3}></Grid>
      </Grid>
      <br />
      <br />

      <Snackbar
        place="tr"
        color="danger"
        icon={AddAlert}
        message={getLocal("Failed to verify")}
        open={error}
        onClose={() => setError(false)}
      />
    </>
  );
}
