import React from "react";
import { Row } from "react-bootstrap";
import { useForm } from "react-hook-form";
import {
  is_valid_Decimal,
  is_valid_Integer,
} from "../../utils/inputValidations";
import { getLegalProfileLookup } from "../../api/legalProfile";
import { getExportAgreementLookup } from "../../api/exportAgreement";
import {
  getAssociationForApply,
  getMembershipRequest,
  saveMembershipApplication,
} from "../../api/membership";
import { useRouter } from "next/router";
import { getLocalByKey } from "../api/menu";
import { Grid, Snackbar } from "@mui/material";
import { AddAlert } from "@mui/icons-material";
import { useSelector } from "react-redux";
import Custom404 from "../404";
import Login from "../login";

const emptyGuid = "00000000-0000-0000-0000-000000000000";
export default function MembershipApplyForm({ translations }) {
  const [user, setUser] = React.useState(
    useSelector((state) => state.auth.user)
  );

  const location = useRouter();
  const { locale } = location;
  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  const [attachments, setAttachments] = React.useState([]);
  //
  const [legalProfiles, setLegalProfiles] = React.useState([]);
  React.useEffect(() => {
    getLegalProfileLookup(1).then((data) => {
      setLegalProfiles(data.data);
    });
  }, []);
  const [exportAgreements, setExportAgreements] = React.useState([]);
  React.useEffect(() => {
    getExportAgreementLookup(1).then((data) => {
      setExportAgreements(data.data);
    });
  }, []);
  //
  const {
    register,
    handleSubmit,
    setValue,
    formState, //: { errors },
  } = useForm();
  //
  let associationIdValue = 0;
  let requestIdValue = null;
  let typeValue = 0;
  if (location.query) {
    const { associationId, requestId, type } = location.query;
    associationIdValue = associationId;
    requestIdValue = requestId;
    typeValue = type;
  }
  const [association, setAssociation] = React.useState(null);
  const [requirements, setRequirements] = React.useState([]);
  React.useEffect(() => {
    if (user && associationIdValue) {
      getAssociationForApply(associationIdValue).then((data) => {
        setAssociation(data.data);
        if (
          data.data &&
          data.data.membershipRequirements &&
          data.data.membershipRequirements.$values
        ) {
          const list = data.data.membershipRequirements.$values.filter(
            (r) =>
              (typeValue == 0 && r.applicationRequirementLevelValue != 0) ||
              (typeValue == 2 && r.renewalRequirementLevelValue != 0)
          );
          setRequirements(list);
        }
      });
    }
  }, [user, associationIdValue]);
  //
  const [request, setRequest] = React.useState([]);
  const [requestDocs, setRequestDocs] = React.useState([]);
  React.useEffect(() => {
    if (user && requestIdValue) {
      getMembershipRequest(requestIdValue).then((data) => {
        setRequest(data.data);
        setRequestDocs(data.data.responses.$values);
      });
    }
  }, [user, requestIdValue]);
  //
  function getInput(r) {
    const idStr = r.id.toString();
    const name = idStr.substr(idStr.length - 15).replace("-", "");
    const responseDoc = requestDocs.find((r) => r.requirementId === idStr);
    const readOnly =
      !requestIdValue ||
      request.applicationStatusValue === 0 ||
      (responseDoc && responseDoc.reviewStatusValue === 3)
        ? ""
        : "readOnly";
    const disabled = readOnly === "" ? "" : "disabled";
    if (r.inputMethodValue === 1) {
      let docLink = <></>;
      if (responseDoc) {
        const fileName = responseDoc.id
          .substr(responseDoc.id.length - 15)
          .replace("-", "");
        const relativePath = `membership/documents/${fileName}${responseDoc.textResponse}`;
        docLink = (
          <a
            href={`${process.env.REACT_APP_API_URL}/${relativePath}`}
            target="_blank"
            rel="noreferrer"
          >
            {getLocal("Saved Document")}
          </a>
        );
      }
      return (
        <>
          {responseDoc && responseDoc.textResponse ? docLink : null}
          {readOnly === "" ? (
            <input
              title={getLocal(r.description, r.descriptionLocal)}
              type="file"
              name={name}
              className="form-control "
              {...register(name)}
              style={{ borderColor: formState.errors[name] ? "red" : "" }}
            />
          ) : null}
        </>
      );
    } else if (r.inputMethodValue === 2) {
      return (
        <input
          {...register(name, { validate: (value) => is_valid_Integer(value) })}
          type="number"
          name={name}
          {...{ readOnly }}
          className="form-control "
          title={
            formState.errors[name]
              ? getLocal("Invalid Number Value")
              : getLocal(r.description, r.descriptionLocal)
          }
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    } else if (r.inputMethodValue === 3) {
      return (
        <input
          {...register(name, { validate: (value) => is_valid_Decimal(value) })}
          type="number"
          name={name}
          {...{ readOnly }}
          className="form-control "
          title={
            formState.errors[name]
              ? getLocal("Invalid Decimal Number")
              : getLocal(r.description, r.descriptionLocal)
          }
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    } else if (r.inputMethodValue === 4) {
      return (
        <input
          {...register(name, { required: true })}
          type="date"
          name={name}
          title={getLocal(r.description, r.descriptionLocal)}
          {...{ readOnly }}
          maxLength="100"
          className="form-control "
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    } else if (r.inputMethodValue === 9) {
      //Invoice and Payment
      /* return (
        <input
          {...register(name)}
          type="text"
          name={name}
          {...{ readOnly }}
          maxLength="100"
          className="form-control "
          title={formState.errors[name] ? "Invalid Input" : ""}
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      ); */
    } else if (r.inputMethodValue === 5) {
      return (
        <input
          {...register(name, { required: true })}
          type="text"
          name={name}
          title={getLocal(r.description, r.descriptionLocal)}
          {...{ readOnly }}
          className="form-control "
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    } else if (r.inputMethodValue === 6) {
      return (
        <textarea
          {...register(name, { required: true })}
          rows="4"
          name={name}
          title={getLocal(r.description, r.descriptionLocal)}
          {...{ readOnly }}
          className="form-control "
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        ></textarea>
      );
    } else if (r.inputMethodValue === 7) {
      //LegalDocument
      return (
        <>
          <select
            {...register(name, { required: true })}
            name={name}
            {...{ disabled }}
            className="form-control "
            title={
              formState.errors[name]
                ? getLocal("Invalid Input")
                : getLocal(r.description, r.descriptionLocal)
            }
            style={{ borderColor: formState.errors[name] ? "red" : "" }}
          >
            {legalProfiles.$values
              ? legalProfiles.$values.map((lp, index) => (
                  <option key={index} value={lp.id}>
                    {lp.name}
                  </option>
                ))
              : null}
          </select>
        </>
      );
    } else if (r.inputMethodValue === 8) {
      //TradeAgreement
      return (
        <>
          <select
            {...register(name, { required: true })}
            name={name}
            {...{ disabled }}
            className="form-control "
            title={
              formState.errors[name]
                ? getLocal("Invalid Input")
                : getLocal(r.description, r.descriptionLocal)
            }
            style={{ borderColor: formState.errors[name] ? "red" : "" }}
          >
            {exportAgreements.$values
              ? exportAgreements.$values.map((lp, index) => (
                  <option key={index} value={lp.id}>
                    {lp.name}
                  </option>
                ))
              : null}
          </select>
        </>
      );
    } else if (r.inputMethodValue === 10) {
      //Lookup
      return (
        <>
          <select
            {...register(name, { required: true })}
            name={name}
            {...{ disabled }}
            className="form-control "
            title={
              formState.errors[name]
                ? getLocal("Invalid Input")
                : getLocal(r.description, r.descriptionLocal)
            }
            style={{ borderColor: formState.errors[name] ? "red" : "" }}
          >
            {association.requirementLookups.$values &&
            association.requirementLookups.$values.find(
              (l) => l.id == r.lookupTypeId
            )
              ? association.requirementLookups.$values
                  .find((l) => l.id == r.lookupTypeId)
                  .lookupValues.$values.map((lp, index) => (
                    <option key={index} value={lp.id}>
                      {lp.name}
                    </option>
                  ))
              : null}
          </select>
        </>
      );
    }
  }
  function setInputValue(r) {
    if (formState && formState.isDirty) return;
    if (r.inputMethodValue !== 1 && r.id !== emptyGuid) {
      const idStr = r.id.toString();
      const name = idStr.substr(idStr.length - 15).replace("-", "");
      const responseDoc = requestDocs.find((r) => r.requirementId === idStr);
      let value = responseDoc ? responseDoc.response : "";
      if (
        responseDoc &&
        (responseDoc.requirement.inputMethodValue === 7 ||
          responseDoc.requirement.inputMethodValue == 8 ||
          responseDoc.requirement.inputMethodValue == 10)
      ) {
        value = responseDoc.lookupValueId;
      }
      setValue(name, value);
    }
  }
  function Redirect() {
    if (requestIdValue) {
      location.push(
        `/myServices/applicationDetailPage?applicationId=${requestIdValue}&associationId=${associationIdValue}`
      );
    } else {
      location.push(`/myServices?navTo=membership`);
    }
  }
  function onSubmit(inputData) {
    var formData = new FormData();
    formData.append("Id", requestIdValue ? requestIdValue : emptyGuid);
    formData.append("associationId", associationIdValue);
    for (const [key, value] of Object.entries(inputData)) {
      const dataValue = value && value[0] && value[0].name ? value[0] : value;
      formData.append(key, dataValue);
    }
    attachments.map((att) => {
      formData.append("AttachmentTitle", att.title);
      formData.append(att.title, att.file);
    });
    saveMembershipApplication(formData)
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
        // console.log(error);
        showNotification("tr");
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
          }, 5000);
        }
        break;
      default:
        break;
    }
  };
  function compare(a, b) {
    if (a.orderNo < b.orderNo) {
      return -1;
    }
    if (a.orderNo > b.orderNo) {
      return 1;
    }
    return 0;
  }
  const getInputLabel = (req) => {
    if (!req || req.inputMethodValue === 9 || req.requirementTypeValue == 2)
      return;
    var value = getLocalByVal(req.label, req.labelLocal);
    if (req.requirementType === "Header") {
      return (
        <label>
          <h3>{value}</h3>
        </label>
      );
    } else {
      return <label>{value}</label>;
    }
  };

  
  if (!user) {
    //return <Custom404 statusCode={404} />;
    return <Login translations={translations} />;
  }
  return (
    <>
      {/* <Topbar/> */}
      {association ? (
        <div className="row">
          <div className="bg">{getLocalByVal(association.nameEng, association.nameLocal)}</div>
          <Grid container spacing={2}>
          <Grid item xs={1} sm={2}></Grid>{" "}
          <Grid item xs={10} sm={6}>
            <form
              onSubmit={handleSubmit(onSubmit)}
              encType="multipart/form-data"
            >
              {requirements
                ? requirements.sort(compare).map((req, index) => (
                    <Row xs={10} md={8} key={index}>
                      {getInputLabel(req)}
                      {getInput(req)} {setInputValue(req)}
                    </Row>
                  ))
                : null}
              <br />
              {/* {association &&
              association.allowAdditionalAttachments &&
              (!requestIdValue || requestIdValue == emptyGuid) ? (
                <>
                  <ServiceRequestFormAttachments
                    attachments={attachments}
                    setAttachments={setAttachments}
                    showNotification={showNotification}
                    setErrorMsg={setErrorMsg}
                  />
                </>
              ) : null} */}
              <br />
              <button type="submit" className="btn btn-success">
              {getLocal("Save")}
              </button>
              <button type="button" className="btn btn-warning" onClick={Redirect}>
              {getLocal("Cancel")}
              </button>
            </form>
            <br />
          </Grid>
          <Grid item xs={1} sm={2}></Grid>
          </Grid>
        </div>
      ) : null}
      <Grid container spacing={2}>
        <br />
        <br />

        <Snackbar
          place="tr"
          color="danger"
          icon={AddAlert}
          message={errorMsg || !error ? errorMsg : getLocal("Failed to Save request")}
          open={error}
          onClose={() => setError(false)}
        />
      </Grid>
    </>
  );
}

// export default App;
