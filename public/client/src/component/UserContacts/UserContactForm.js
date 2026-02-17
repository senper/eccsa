import React, { useState } from "react";
import { Grid, Snackbar } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import { AddAlert } from "@material-ui/icons";
import "../../App.css";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../../component/Footer/Footer";
import { useLocation } from "react-router-dom";
import { useForm } from "react-hook-form";
import { useHistory } from "react-router-dom";
import {
    ContactTypes,
  getUserContact,
  saveUserContact,
} from "../../services/userContact";
import { Row, Col, Form } from "react-bootstrap";
import { makeStyles } from "@material-ui/core/styles";

const useStyle = makeStyles((theme) => ({
  grid: {
    width: "100%",
    margin: "15px",
    padding: "1px",
    row: "20px",
    background: "#fff",
  },
  main: {
    width: "100%",
    background: "#fff",
    padding: "10px",
  },
  paper: {
    // padding:theme.spacing(1),
    textAlign: "left",
    color: "#198754",
    background: "#aaf",
    padding: "10px",
  },
  appBar: {
    color: "#000",
    background: "#fff",
  },
  margin: {
    marginLeft: "100px",
    marginTop: "5px",
    color: "primary",
  },
}));

const emptyGuid = "00000000-0000-0000-0000-000000000000";
export default function UserContactForm() {
  window.scrollTo({
    top: 0,
  });
  const classes = useStyle();
  const location = useLocation();
  const history = useHistory();
  const {
    register,
    handleSubmit,
    setValue,
    getValues,
    formState, //: { errors },
  } = useForm();
  let serviceTypeIdValue = 0;
  let idValue = null;
  if (location.state) {
    const { id } = location.state;
    idValue = id;
  }
  const [userContact, setUserContact] = React.useState([]);
  const [readOnly, setReadOnly] = React.useState("");
  const [disabled, setDisabled] = React.useState("");
  React.useEffect(() => {
    if (idValue) {
      getUserContact(idValue).then((data) => {
        setUserContact(data.data);
        const values = getValues();
        for (const [key, value] of Object.entries(values)) {
          setValue(key, data.data[key]);
        }
        const canEdit = !idValue || !data.data || !data.data.isLocked;
        setReadOnly(canEdit ? "" : "readOnly");
        setDisabled(canEdit ? "" : "disabled");
      });
    }
  }, [idValue]);
  function onSubmit(inputData) {
    saveUserContact(inputData)
      .then((data) => {
        if (data.data.saved) {
          Redirect();
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
          showNotification("tr");
        }
      })
      .catch((error) => {
        console.log(error.response);
        showNotification("tr");
      });
  }
  function Redirect() {
    history.push({
      pathname: "/myServices",
      state: {
        navTo: "contacts",
      },
    });
  }
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(null);
  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMsg(null);
          }, 6000);
        }
        break;
      default:
        break;
    }
  };
  return (
    <div className={classes.main}>
      <Grid container spacing={2} className="grid">
        <Grid item xs={3} md={3} className={classes.counter}></Grid>
        <Grid item xs={6} md={6} className={classes.counter}>
          <div class="row">
            <div class="bg">{idValue ? "Update" : "Add"} Contact</div>
          </div>
          <Form onSubmit={handleSubmit(onSubmit)} enctype="multipart/form-data">
            <input type="hidden" {...register("id")} />
            {setValue("id", idValue ? idValue : emptyGuid)}
            <Row className="g-2">
              <Col md>
                <Form.Label> Priority</Form.Label>
                <Form.Control
                  type="number"
                  {...register("priority", {
                    required: true,
                    maxLength: 50,
                  })}
                  style={{
                    borderColor: formState.errors["priority"] ? "red" : "",
                  }}
                />
              </Col>
              <Col md>
              <Form.Label> Type</Form.Label>
                <Form.Select
                  {...register("contactType", { required: true })}
                  style={{
                    borderColor: formState.errors["contactType"]
                      ? "red"
                      : "",
                  }}
                >
                  <option></option>
                  {ContactTypes.map((value) => (
                    <option value={value.name}>{value.name}</option>
                  ))}
                </Form.Select>
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> Address</Form.Label>
                <Form.Control
                  type="text"
                  {...{ readOnly }}
                  {...register("address", { required: true, maxLength: 255 })}
                  style={{
                    borderColor: formState.errors["address"] ? "red" : "",
                  }}
                />
              </Col>
            </Row>
            <button type="submit" class="btn btn-success">
              Save
            </button>
            <button type="button" class="btn btn-warning" onClick={Redirect}>
              Cancel
            </button>
            <br />
          </Form>
        </Grid>
        <Grid item xs={3} md={3} className={classes.counter}></Grid>
        <Grid container spacing={2}>
          <br />
          <br />
        </Grid>
  
        <Snackbar
          place="tr"
          color="danger"
          icon={AddAlert}
          message={errorMsg ? errorMsg : "Failed to Save"}
          open={error}
          closeNotification={() => setError(false)}
          close
        />
      </Grid>
    </div>
  );
}

// export default App;
