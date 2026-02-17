import Link from "next/link";

export default function Custom404() {
  return (
    <div id="notfound">
      <div className="notfound">
        <div className="notfound-404">
          <h1></h1>
        </div>
        <h2>We are sorry, Page not found!</h2>
        <p>
          The page you are looking for might have been removed had its name
          changed or is temporarily unavailable.
        </p>
        <Link href="/">Back To Homepage</Link>
      </div>
    </div>
  );
}
