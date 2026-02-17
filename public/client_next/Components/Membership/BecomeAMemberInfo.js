import { useRouter } from "next/router";
import React from "react";
import { Button } from "react-bootstrap";
const toasts = [
  {
    id: "Networking",
    category: "Networking",
    message:
      "To find local and foreign markets for their products and  services.",
  },
  {
    id: "Capacity",
    category: "Capacity Building",
    message:
      "in the area of training, manual preparation, strategic plan document designing, and the like)",
  },
  {
    category: "Consultancy",
    message: "membership and business development consultancy",
  },
  {
    category: "Experience Sharing",
    message: "exchange information and share experience etc.",
  },
];

function Toast(props) {
  return (
    <div className={`Toast Toast--${props.category}`}>
      <main className="Toast__message">
        <header className="Toast__message-category">{props.category}</header>
        <p className="Toast__message-text">{props.message}</p>
      </main>
    </div>
  );
}
export default function BecomeAMemberInfo({ getLocal, getLocalByVal }) {
  var token = localStorage.getItem("access_token");
  const history = useRouter();
  function becomeAMember() {
    return token
      ? history.push("/myServices?navTo=membership")
      : history.push("/login");
  }
  return (
    <>
      <div className="bg">{getLocal("Membership")}</div>
      {toasts.map((toast, i) => (
        <Toast category={toast.category} key={i} message={toast.message} />
      ))}
      <Button
        type="submit"
        style={{
          float: "none",
          marginTop: "0",
          marginBottom: "0",
          marginLeft: "40%",
        }}
        variant="outline-secondary"
        size="sm"
        id="button-become-a-member"
        onClick={() => becomeAMember()}
      >
        {getLocal("Become A Member")}
      </Button>
    </>
  );
}
