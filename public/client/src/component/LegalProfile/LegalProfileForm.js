import React from "react";
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
  getLegalProfile,
  LegalDocumentTypes,
  saveLegalProfile,
} from "../../services/legalProfile";
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
export default function LegalProfileForm() {
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
  let idValue = null;
  if (location.state) {
    const { id } = location.state;
    idValue = id;
  }
  const [legalProfile, setLegalProfile] = React.useState([]);
  const [readOnly, setReadOnly] = React.useState("");
  const [disabled, setDisabled] = React.useState("");
  React.useEffect(() => {
    if (idValue) {
      getLegalProfile(idValue).then((data) => {
        setLegalProfile(data.data);
        const values = getValues();
        for (const [key] of Object.entries(values)) {
          setValue(key, data.data[key]);
        }
        const canEdit = !idValue || !data.data || !data.data.isLocked;
        setReadOnly(canEdit ? "" : "readOnly");
        setDisabled(canEdit ? "" : "disabled");
      });
    }
  }, [idValue]);
  function onSubmit(inputData) {
    var formData = new FormData();
    formData.append("id", idValue ? idValue : emptyGuid);
    for (const [key, value] of Object.entries(inputData)) {
      const dataValue = value && value[0] && value[0].name ? value[0] : value;
      formData.append(key, dataValue);
    }
    saveLegalProfile(formData)
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
        showNotification("tr");
      });
  }
  function Redirect() {
    history.push({
      pathname: "/myServices",
      state: {
        navTo: "legalProfile",
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
  const getAttachmentLink = () => {
    if (!legalProfile || !legalProfile.fileExtension) {
      return null;
    }
    const fileName = legalProfile.id
      .substr(legalProfile.id.length - 15)
      .replace("-", "");
    const userIdPath = legalProfile.userId
      .substr(legalProfile.id.length - 15)
      .replace("-", "");
    const relativePath = `Users/LegalProfiles/${userIdPath}/${fileName}${legalProfile.fileExtension}`;
    return (
      <>
        {" "}
        &nbsp;&nbsp;&nbsp;&nbsp;
        <a
          href={`${process.env.REACT_APP_API_URL}/${relativePath}`}
          target="_blank"
          rel="noreferrer"
        >
          Saved Document
        </a>
      </>
    );
  };
  return (
    <div className={classes.main}>
      <Grid container spacing={2} className="grid">
        <Grid item xs={3} md={3} className={classes.counter}></Grid>
        <Grid item xs={6} md={6} className={classes.counter}>
          <div class="row">
            <div class="bg">{idValue ? "Update" : "Add"} Legal Profile</div>
          </div>
          <Form onSubmit={handleSubmit(onSubmit)} enctype="multipart/form-data">
            <Row className="g-2">
              <Col md>
                <Form.Label> Type</Form.Label>
                <Form.Select
                  {...{ disabled }}
                  {...register("legalDocumentTypeValue", { required: true })}
                  style={{
                    borderColor: formState.errors["legalDocumentType"]
                      ? "red"
                      : "",
                  }}
                  onChange={(event) => {
                    if (!idValue) {
                      const selected = LegalDocumentTypes.find(
                        (lt) => lt.value == event.target.value
                      );
                      if (selected) {
                        setValue("title", selected.name);
                      }
                    }
                  }}
                >
                  <option></option>
                  {LegalDocumentTypes.map((value) => (
                    <option value={value.value}>{value.name}</option>
                  ))}
                </Form.Select>
              </Col>
              <Col md>
                <Form.Label> Title</Form.Label>
                <Form.Control
                  type="text"
                  {...{ readOnly }}
                  {...register("title", { required: true, maxLength: 50 })}
                  style={{
                    borderColor: formState.errors["title"] ? "red" : "",
                  }}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> Document Number </Form.Label>
                <Form.Control
                  {...{ readOnly }}
                  type="documentNumber"
                  {...register("documentNumber", {
                    maxLength: 50,
                  })}
                  style={{
                    borderColor: formState.errors["documentNumber"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label> Expiry Date </Form.Label>
                <Form.Control
                  {...{ readOnly }}
                  type="date"
                  {...register("expiryDateStr")}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Group controlId="formFile" className="mb-3">
                  <Form.Label>
                    Attachment
                    {getAttachmentLink()}
                  </Form.Label>
                  <Form.Control
                    {...{ disabled }}
                    type="file"
                    {...register("attachment", { required: !legalProfile })}
                  />
                </Form.Group>
              </Col>
              <Col md>
                <Form.Label> Status</Form.Label>
                <Form.Select
                  {...register("statusValue", { required: true })}
                  style={{
                    borderColor: formState.errors["status"] ? "red" : "",
                  }}
                >
                  <option value="1">Active</option>
                  <option value="0">Inactive</option>
                </Form.Select>
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
